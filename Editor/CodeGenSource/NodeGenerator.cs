using GBG.VisualPlayable.Attribute;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using Unity.VisualScripting;
using UnityEditor;

namespace GBG.VisualPlayable.Editor.CodeGen
{
    public readonly struct TypeSource
    {
        public readonly string Name;

        public readonly string Content;


        public TypeSource(string name, string content)
        {
            Name = name;
            Content = content;
        }
    }


    public static class NodeGenerator
    {
        private static readonly IReadOnlyList<Type> _visualPlayableTypes = new List<Type>
        {
            typeof(AnimationBrain), typeof(AnimationLayer)
        };

        [MenuItem("Tools/Bamboo/Visual Playable/[DEV] Regenerate Visual Playable Node Source Code")]
        public static void RegenerateVisualPlayableNodes()
        {
            const string codeGenParentFolder = "Packages/com.greenbamboogames.visualplayable/Runtime/Scripts";
            const string codeGenFolderName = "Generated";
            var codeGenFolder = $"{codeGenParentFolder}/{codeGenFolderName}";

            // Clear old code
            AssetDatabase.DeleteAsset(codeGenFolder);
            AssetDatabase.CreateFolder(codeGenParentFolder, codeGenFolderName);

            foreach (Type visualPlayableType in _visualPlayableTypes)
            {
                var visualPlayableTypeFolder = $"{codeGenFolder}/{visualPlayableType.Name}";
                if (!AssetDatabase.IsValidFolder(visualPlayableTypeFolder))
                {
                    AssetDatabase.CreateFolder(codeGenFolder, visualPlayableType.Name);
                }

                var nodeSrcList = Generate(visualPlayableType);
                foreach (var nodeSrc in nodeSrcList)
                {
                    var codeFilePath = $"{visualPlayableTypeFolder}/{nodeSrc.Name}.cs";
                    var codeFileContent = nodeSrc.Content;
                    File.WriteAllText(codeFilePath, codeFileContent);
                }
            }

            AssetDatabase.Refresh();
        }


        private const string _INPUT_TRIGGER_NAME = "InputTrigger";

        private const string _OUTPUT_TRIGGER_NAME = "OutputTrigger";

        private const string _FAILURE_OUTPUT_TRIGGER_NAME = "FailureOutputTrigger";


        public static List<TypeSource> Generate(Type type)
        {
            if (type.ContainsGenericParameters)
            {
                throw new ArgumentException($"Type '{type.Name}' contains generic parameter.", nameof(type));
            }

            var srcList = new List<TypeSource>(30);

            var ctors = type.GetConstructors(BindingFlags.Instance | BindingFlags.Public);
            foreach (var ctor in ctors)
            {
                var src = Generate(ctor);
                srcList.Add(src);
            }

            var methods = type.GetMethods(BindingFlags.Instance | BindingFlags.Public);
            foreach (var method in methods)
            {
                if (method.IsSpecialName) // Methods which can't be called by users(eg. property get/set methods)
                {
                    continue;
                }

                if (method.DeclaringType == typeof(Object))
                {
                    continue;
                }

                if (method.GetAttribute<ExcludeMethodFromNodeAttribute>() != null)
                {
                    continue;
                }

                var src = Generate(method);
                srcList.Add(src);
            }

            return srcList;
        }

        public static TypeSource Generate(MethodBase method)
        {
            // Check Property
            if (method.IsSpecialName && !method.Name.Equals(".ctor"))
            {
                throw new ArgumentException($"Method '{method.Name}' has special name.", nameof(method));
            }

            var reflectedType = method.ReflectedType;
            if (reflectedType == null)
            {
                throw new ArgumentException($"Reflected type of method '{method.Name}' is null.", nameof(method));
            }

            var reflectedTypeName = GetTypeName(reflectedType);

            var srcBuilder = new StringBuilder();
            srcBuilder.AppendLine("// AUTO GENERATED CODE");
            srcBuilder.AppendLine("// ReSharper disable InconsistentNaming");
            srcBuilder.AppendLine("// ReSharper disable RedundantUsingDirective");

            // Using namespace
            var usingSet = new HashSet<string>()
            {
                "using System;",
                "using System.Collections;",
                "using System.Collections.Generic;",
                "using UnityEngine;",
                "using UnityEngine.Playables;",
                "using UnityEngine.Animations;",
                "using Unity.VisualScripting;",
            };

            CollectUsingNamespace(method, usingSet);
            foreach (var ns in usingSet)
            {
                srcBuilder.AppendLine(ns);
            }

            // Class namespace
            srcBuilder.AppendLine();
            srcBuilder.AppendLine("namespace GBG.VisualPlayable.VisualNode").AppendLine("{");

            srcBuilder.AppendLine($"\t// {reflectedType}.{method.Name}");

            // Class attributes
            string classNameSuffix = method.IsConstructor ? "Ctor" : method.Name;
            var classSuffixAttr = method.GetAttribute<NodeSuffixAttribute>();
            if (classSuffixAttr != null)
            {
                var nodeSuffix = classSuffixAttr.Suffix.Replace(' ', '_');
                classNameSuffix = $"{classNameSuffix}_{nodeSuffix}";
            }

            var nicifyTypeName = ObjectNames.NicifyVariableName(reflectedTypeName);
            var nicifyMethodName = ObjectNames.NicifyVariableName(method.Name);
            var nodeSuffixAttr = method.GetAttribute<NodeSuffixAttribute>();
            string nodeTitle;
            if (nodeSuffixAttr != null)
            {
                nodeTitle = method.IsConstructor
                    ? $"Create {nicifyTypeName}"
                    : $"{nicifyMethodName}({nodeSuffixAttr.Suffix})";
            }
            else
            {
                nodeTitle = method.IsConstructor ? $"Create {nicifyTypeName}" : nicifyMethodName;
            }

            srcBuilder.AppendLine($"[UnitCategory(\"Visual Playable/{nicifyTypeName}\")]")
                .AppendLine($"[UnitTitle(\"{nodeTitle}\")]");

            // Class start
            var className = $"Node_{reflectedTypeName}_{classNameSuffix}";
            srcBuilder.AppendLine($"internal class {className} : Unit").AppendLine("{");

            // Input trigger
            srcBuilder.AppendLine("\t// Input trigger");
            srcBuilder.AppendLine("[PortLabelHidden][DoNotSerialize]")
                .AppendLine($"public ControlInput {_INPUT_TRIGGER_NAME} {{ get; private set; }}");

            // Output trigger
            srcBuilder.AppendLine();
            srcBuilder.AppendLine("\t// Output trigger");
            var failureOutputAttr = method.GetAttribute<FailureOutputTriggerAttributeBase>();
            if (failureOutputAttr == null)
            {
                srcBuilder.AppendLine("[PortLabelHidden][DoNotSerialize]");
                srcBuilder.AppendLine($"public ControlOutput {_OUTPUT_TRIGGER_NAME} {{ get; private set; }}");
            }
            else
            {
                srcBuilder.AppendLine($"[PortLabel(\"{failureOutputAttr.SuccessLabel}\")]");
                srcBuilder.AppendLine("[DoNotSerialize]");
                srcBuilder.AppendLine($"public ControlOutput {_OUTPUT_TRIGGER_NAME} {{ get; private set; }}");

                srcBuilder.AppendLine($"[PortLabel(\"{failureOutputAttr.FailureLabel}\")]");
                srcBuilder.AppendLine("[DoNotSerialize]");
                srcBuilder.AppendLine($"public ControlOutput {_FAILURE_OUTPUT_TRIGGER_NAME} {{ get; private set; }}");
            }

            // Input params
            srcBuilder.AppendLine();
            srcBuilder.AppendLine("\t// Input params");
            if (!method.IsConstructor) // Instance constructor
            {
                srcBuilder.AppendLine("\t// Input instance");
                var nicifyParamName = ObjectNames.NicifyVariableName(reflectedTypeName);
                var inputParamName = GetInputParamName(reflectedTypeName);
                srcBuilder.AppendLine($"[PortLabel(\"{nicifyParamName}\")]")
                    .AppendLine("[DoNotSerialize]")
                    .AppendLine($"public ValueInput {inputParamName} {{ get; private set; }}");
            }

            var parameters = method.GetParameters();
            for (int i = 0; i < parameters.Length; i++)
            {
                if (i != 0)
                {
                    srcBuilder.AppendLine();
                }

                ParameterInfo param = parameters[i];
                srcBuilder.AppendLine($"\t// Input parameter: {param.Name}");
                var nicifyParamName = ObjectNames.NicifyVariableName(param.Name);
                var inputParamName = GetInputParamName(param.Name);
                srcBuilder.AppendLine($"[PortLabel(\"{nicifyParamName}\")]")
                    .AppendLine("[DoNotSerialize]")
                    .AppendLine($"public ValueInput {inputParamName} {{ get; private set; }}");
            }

            // Output params
            srcBuilder.AppendLine();
            srcBuilder.AppendLine("\t// Output params");
            var methodInfo = method as MethodInfo;
            var returnType = methodInfo?.ReturnType ?? reflectedType;
            var returnTypeName = GetTypeName(returnType);
            if (returnType != typeof(void))
            {
                srcBuilder.AppendLine($"\t// Output(return value): {returnTypeName}");
                var labelAttr = methodInfo?.GetAttribute<OutputPortLabelAttribute>();
                var outputPortLabel = labelAttr != null
                    ? labelAttr.Label
                    : ObjectNames.NicifyVariableName(returnTypeName);
                var outputParamName = GetOutputParamName(returnTypeName);
                srcBuilder.AppendLine($"[PortLabel(\"{outputPortLabel}\")]")
                    .AppendLine("[DoNotSerialize]")
                    .AppendLine($"public ValueOutput {outputParamName} {{ get; private set; }}");
            }

            // Definition method start
            srcBuilder.AppendLine();
            srcBuilder.AppendLine("protected override void Definition()").AppendLine("{");

            // Definition - InputTrigger
            srcBuilder.AppendLine("\t// InputTrigger");
            srcBuilder.AppendLine($"{_INPUT_TRIGGER_NAME} = ControlInput(nameof({_INPUT_TRIGGER_NAME}), OnTriggered);");

            // Definition - OutputTrigger
            srcBuilder.AppendLine("\t// OutputTrigger");
            srcBuilder.AppendLine($"{_OUTPUT_TRIGGER_NAME} = ControlOutput(nameof({_OUTPUT_TRIGGER_NAME}));");
            if (failureOutputAttr != null)
            {
                srcBuilder.AppendLine($"{_FAILURE_OUTPUT_TRIGGER_NAME} = " +
                                      $"ControlOutput(nameof({_FAILURE_OUTPUT_TRIGGER_NAME}));");
            }

            // Definition - Input parameters
            srcBuilder.AppendLine();
            srcBuilder.AppendLine("\t// Input parameters");
            if (!method.IsConstructor && !method.IsStatic) // Instance
            {
                var inputParamName = GetInputParamName(reflectedTypeName);
                var inputParamType = reflectedTypeName;
                srcBuilder.AppendLine($"{inputParamName} = ValueInput<{inputParamType}>(nameof({inputParamName}));");
            }

            foreach (var param in parameters)
            {
                var inputParamName = GetInputParamName(param.Name);
                var inputParamType = GetTypeName(param.ParameterType);
                if (param.HasDefaultValue)
                {
                    srcBuilder.AppendLine($"{inputParamName} = " +
                                          $"ValueInput<{inputParamType}>(nameof({inputParamName}), " +
                                          $"{GetParamDefaultValueString(param)});");
                }
                else
                {
                    srcBuilder.AppendLine($"{inputParamName} = " +
                                          $"ValueInput<{inputParamType}>(nameof({inputParamName}));");
                }
            }

            // Definition - Output parameters
            srcBuilder.AppendLine();
            if (returnType != typeof(void))
            {
                srcBuilder.AppendLine("\t// Output parameters");
                var outputParamName = GetOutputParamName(returnTypeName);
                srcBuilder.AppendLine($"{outputParamName}=ValueOutput<{returnTypeName}>(nameof({outputParamName}));");
            }

            // Definition - Requirements
            srcBuilder.AppendLine();
            srcBuilder.AppendLine("\t// Requirements");
            foreach (var param in parameters)
            {
                if (!param.HasDefaultValue)
                {
                    var inputParamName = GetInputParamName(param.Name);
                    srcBuilder.AppendLine($"Requirement({inputParamName}, {_INPUT_TRIGGER_NAME});");
                }
            }

            // Definition - Successions
            srcBuilder.AppendLine();
            srcBuilder.AppendLine("\t// Successions");
            srcBuilder.AppendLine($"Succession({_INPUT_TRIGGER_NAME}, {_OUTPUT_TRIGGER_NAME});");
            if (failureOutputAttr != null)
            {
                srcBuilder.AppendLine($"Succession({_INPUT_TRIGGER_NAME}, {_FAILURE_OUTPUT_TRIGGER_NAME});");
            }

            // TODO: Definition - Assignments
            srcBuilder.AppendLine();
            if (returnType != typeof(void))
            {
                srcBuilder.AppendLine("\t// Assignments");
                var outputParamName = GetOutputParamName(returnTypeName);
                srcBuilder.AppendLine($"Assignment({_INPUT_TRIGGER_NAME}, {outputParamName});");
                srcBuilder.AppendLine();
            }

            // Definition method end
            srcBuilder.AppendLine("}");

            // TODO: OnTriggered method start
            srcBuilder.AppendLine();
            srcBuilder.AppendLine("private ControlOutput OnTriggered(Flow flow)").AppendLine("{");
            if (failureOutputAttr != null)
            {
                srcBuilder.AppendLine("var failed = false;");
            }

            srcBuilder.AppendLine("try{");

            // OnTriggered - Input parameter values
            srcBuilder.AppendLine("\t// Input parameter values");
            if (!method.IsConstructor && !method.IsStatic) // Instance
            {
                var inputParamName = GetInputParamName(reflectedTypeName);
                var inputParamType = reflectedTypeName;
                var inputValueName = GetInputValueName(reflectedTypeName);
                srcBuilder.AppendLine($"var {inputValueName} = flow.GetValue<{inputParamType}>({inputParamName});");
            }

            foreach (var param in parameters)
            {
                var inputParamName = GetInputParamName(param.Name);
                var inputParamType = GetTypeName(param.ParameterType);
                var inputValueName = GetInputValueName(param.Name);
                srcBuilder.AppendLine($"var {inputValueName} = flow.GetValue<{inputParamType}>({inputParamName});");
            }

            // OnTriggered - Output parameter values
            srcBuilder.AppendLine();
            var callMethodCode = GenerateCallMethodCode(method, parameters);
            if (returnType == typeof(void))
            {
                srcBuilder.AppendLine(callMethodCode);
            }
            else
            {
                srcBuilder.AppendLine("\t// Output parameter values");
                var outputValueName = GetOutputValueName(returnTypeName);
                var outputParamName = GetOutputParamName(returnTypeName);
                srcBuilder.AppendLine($"var {outputValueName} = {callMethodCode}");

                // Check failure
                if (failureOutputAttr != null)
                {
                    switch (failureOutputAttr)
                    {
                        case FailureOutputTriggerOnExceptionAttribute:
                            // Not here
                            break;

                        case FailureOutputTriggerOnReturnFalseAttribute:
                            srcBuilder.AppendLine($"failed = !{outputValueName};");
                            break;

                        case FailureOutputTriggerOnReturnMinusAttribute:
                            srcBuilder.AppendLine($"failed = {outputValueName} < 0;");
                            break;

                        case FailureOutputTriggerOnReturnNullAttribute:
                            srcBuilder.AppendLine($"failed = {outputValueName} == null;");
                            break;

                        default:
                            throw new ArgumentOutOfRangeException(nameof(failureOutputAttr),
                                $"Unknown attribute type '{failureOutputAttr}'.");
                    }
                }

                srcBuilder.AppendLine($"flow.SetValue({outputParamName}, {outputValueName});");
            }

            srcBuilder.AppendLine("} catch(Exception e) {");
            if (failureOutputAttr is FailureOutputTriggerOnExceptionAttribute)
            {
                srcBuilder.AppendLine("failed = true;");
            }

            srcBuilder.AppendLine("Debug.LogException(e); }");

            // OnTriggered - Return
            srcBuilder.AppendLine();
            if (failureOutputAttr != null)
            {
                srcBuilder.AppendLine($"if(failed) {{ return {_FAILURE_OUTPUT_TRIGGER_NAME}; }}");
            }

            srcBuilder.AppendLine($"return {_OUTPUT_TRIGGER_NAME};");

            // OnTriggered method end
            srcBuilder.AppendLine("}");
            srcBuilder.AppendLine();

            // Class end and namespace end
            srcBuilder.AppendLine("}").AppendLine("}").AppendLine();

            var classContent = srcBuilder.ToString();
            var classSource = new TypeSource(className, classContent);

            return classSource;
        }


        private static void CollectUsingNamespace(MethodBase method, HashSet<string> usingSet)
        {
            var parameters = method.GetParameters();
            foreach (var param in parameters)
            {
                var paramNs = param.ParameterType.Namespace;
                usingSet.Add($"using {paramNs};");
            }

            if (method.IsConstructor && method.ReflectedType != null)
            {
                var typeNs = method.ReflectedType.Namespace;
                usingSet.Add($"using {typeNs};");
            }
            else if (method is MethodInfo methodInfo && methodInfo.ReturnType != typeof(void))
            {
                var returnTypeNs = methodInfo.ReturnType.Namespace;
                usingSet.Add($"using {returnTypeNs};");
            }
        }

        private static string GetInputParamName(string name) => $"Input_{name}";

        private static string GetInputValueName(string name) => $"Value_{GetInputParamName(name)}";

        private static string GetOutputParamName(string name) => $"Output_{name}";

        private static string GetOutputValueName(string name) => $"Value_{GetOutputParamName(name)}";

        private static string GetTypeName(Type type)
        {
            if (type.ContainsGenericParameters)
            {
                throw new ArgumentException($"Type '{type.Name}' contains generic parameter.", nameof(type));
            }

            if (!type.IsGenericType)
            {
                return type.Name;
            }

            var nameBuilder = new StringBuilder();
            nameBuilder.Append(type.Name.Substring(0, type.Name.IndexOf('`'))).Append("<");
            var genericArgs = type.GetGenericArguments();
            for (int i = 0; i < genericArgs.Length; i++)
            {
                if (i != 0)
                {
                    nameBuilder.Append(", ");
                }

                // genericArgs[i].ReflectedType is null
                nameBuilder.Append(genericArgs[i].Name);
            }

            nameBuilder.Append(">");

            return nameBuilder.ToString();
        }

        private static string GetParamDefaultValueString(ParameterInfo paramInfo)
        {
            if (!paramInfo.HasDefaultValue)
            {
                throw new ArgumentException($"Parameter '{paramInfo.Name}' doesn't have default value.",
                    nameof(paramInfo));
            }

            if (paramInfo.DefaultValue == null)
            {
                return "null";
            }

            if (paramInfo.ParameterType == typeof(float))
            {
                return $"{paramInfo.DefaultValue}f";
            }

            return paramInfo.DefaultValue.ToString().ToLowerInvariant();
        }

        private static string GenerateCallMethodCode(MethodBase method, ParameterInfo[] parameters)
        {
            // Properties
            if ((method.MemberType & MemberTypes.Property) != 0)
            {
                throw new ArgumentException("Can not handle property.", nameof(method));
            }

            var reflectedType = method.ReflectedType;
            if (reflectedType == null)
            {
                throw new ArgumentException($"Reflected type of method '{method.Name}' is null.", nameof(method));
            }

            // Parameters
            var paramBuilder = new StringBuilder();
            for (int i = 0; i < parameters.Length; i++)
            {
                if (i > 0)
                {
                    paramBuilder.Append(", ");
                }

                var param = parameters[i];
                var inputValueName = GetInputValueName(param.Name);
                paramBuilder.Append(inputValueName);
            }

            var reflectedTypeName = GetTypeName(reflectedType);

            // Constructor
            if (method.IsConstructor)
            {
                return $"new {reflectedTypeName}({paramBuilder.ToString()});";
            }

            if (method is MethodInfo)
            {
                var instanceName = GetInputValueName(reflectedTypeName);

                // Static method
                if (method.IsStatic)
                {
                    return $"{reflectedTypeName}.{method.Name}({paramBuilder.ToString()});";
                }

                // Instance method
                return $"{instanceName}.{method.Name}({paramBuilder.ToString()});";
            }

            throw new ArgumentException("Unknown method type.", nameof(method));
        }
    }
}