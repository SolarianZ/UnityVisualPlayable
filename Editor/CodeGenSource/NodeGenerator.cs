using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace GBG.VisualPlayable.Editor.CodeGen
{
    public static class NodeGenerator
    {
        [MenuItem("Tests/TestNodeGen")]
        public static void TestNodeGen()
        {
            var nodeSrcList = Generate(typeof(AnimationBrain));
            foreach (string nodeSrc in nodeSrcList)
            {
                Debug.Log(nodeSrc);
            }
        }


        private const string _INPUT_TRIGGER_NAME = "InputTrigger";

        private const string _OUTPUT_TRIGGER_NAME = "OutputTrigger";


        public static List<string> Generate(Type type)
        {
            var srcList = new List<string>(30);

            var ctors = type.GetConstructors(BindingFlags.Instance | BindingFlags.Public);
            foreach (var ctor in ctors)
            {
                var src = Generate(type, ctor);
                srcList.Add(src);
            }

            var methods = type.GetMethods(BindingFlags.Instance | BindingFlags.Public);
            foreach (var method in methods)
            {
                var src = Generate(type, method);
                srcList.Add(src);
            }

            return srcList;
        }

        public static string Generate(Type type, MethodBase method)
        {
            var usingSet = new HashSet<string>()
            {
                "using System;", "using Unity.VisualScripting;", "using UnityEngine;",
            };

            CollectUsingNamespace(method, usingSet);

            var srcBuilder = new StringBuilder();
            srcBuilder.AppendLine("// AUTO GENERATED CODE");
            srcBuilder.AppendLine("// ReSharper disable InconsistentNaming");

            // Using namespace
            foreach (var ns in usingSet)
            {
                srcBuilder.AppendLine(ns);
            }

            // Namespace
            srcBuilder.AppendLine("namespace GBG.VisualPlayable.VisualNode").AppendLine("{");

            // Class attributes
            var isCtor = method.Name.Equals(".ctor");
            var nicifyTypeName = ObjectNames.NicifyVariableName(type.Name);
            var nicifyMethodName = ObjectNames.NicifyVariableName(method.Name);
            var nodeTitle = isCtor ? $"Create {nicifyTypeName}" : nicifyMethodName;
            srcBuilder.AppendLine($"[UnitCategory(\"Visual Playable/{nicifyTypeName}\")]")
                .AppendLine($"[UnitTitle(\"{nodeTitle}\")]");

            // Class start
            var classNameSuffix = isCtor ? "Ctor" : method.Name;
            srcBuilder.AppendLine($"internal class Node_{type.Name}_{classNameSuffix} : Unit").AppendLine("{");

            // Input trigger
            srcBuilder.AppendLine("\t// Input trigger");
            srcBuilder.AppendLine("[PortLabelHidden][DoNotSerialize]")
                .AppendLine("public ControlInput InputTrigger { get; private set; }")
                .AppendLine();

            // Output trigger
            srcBuilder.AppendLine("\t// Output trigger");
            srcBuilder.AppendLine("[PortLabelHidden][DoNotSerialize]")
                .AppendLine("public ControlOutput OutputTrigger { get; private set; }")
                .AppendLine();

            // Input params
            srcBuilder.AppendLine("\t// Input params");

            if (!isCtor && !method.IsStatic)
            {
                var nicifyParamName = ObjectNames.NicifyVariableName(type.Name);
                var inputParamName = GetInputParamName(type.Name);
                srcBuilder.AppendLine($"[PortLabel(\"{nicifyParamName}\")]")
                    .AppendLine("[DoNotSerialize]")
                    .AppendLine($"public ValueInput {inputParamName} {{ get; private set; }}")
                    .AppendLine();
            }

            var parameters = method.GetParameters();
            foreach (var param in parameters)
            {
                var nicifyParamName = ObjectNames.NicifyVariableName(param.Name);
                var inputParamName = GetInputParamName(param.Name);
                srcBuilder.AppendLine($"[PortLabel(\"{nicifyParamName}\")]")
                    .AppendLine("[DoNotSerialize]")
                    .AppendLine($"public ValueInput {inputParamName} {{ get; private set; }}")
                    .AppendLine();
            }

            // Output params
            srcBuilder.AppendLine("\t// Output params");
            var returnType = (method as MethodInfo)?.ReturnType ?? type;
            if (returnType != typeof(void))
            {
                var nicifyReturnTypeName = ObjectNames.NicifyVariableName(returnType.Name);
                var outputParamName = GetOutputParamName(returnType.Name);
                srcBuilder.AppendLine($"[PortLabel(\"{nicifyReturnTypeName}\")]")
                    .AppendLine("[DoNotSerialize]")
                    .AppendLine($"public ValueOutput {outputParamName} {{ get; private set; }}")
                    .AppendLine();
            }

            // Definition method start
            srcBuilder.AppendLine("protected override void Definition()").AppendLine("{");

            // Definition - InputTrigger & OutputTrigger
            srcBuilder.AppendLine("\t// InputTrigger & OutputTrigger");
            srcBuilder.AppendLine("InputTrigger = ControlInput(nameof(InputTrigger), OnTriggered);");
            srcBuilder.AppendLine("OutputTrigger = ControlOutput(nameof(OutputTrigger));");
            srcBuilder.AppendLine();

            // Definition - Input parameters
            srcBuilder.AppendLine("\t// Input parameters");
            foreach (var param in parameters)
            {
                var inputParamName = GetInputParamName(param.Name);
                var inputParamType = param.ParameterType.Name;
                srcBuilder.AppendLine($"{inputParamName} = ValueInput<{inputParamType}>(nameof({inputParamName}));");
            }

            srcBuilder.AppendLine();

            // Definition - Output parameters
            if (returnType != typeof(void))
            {
                srcBuilder.AppendLine("\t// Output parameters");
                var outputParamName = GetOutputParamName(returnType.Name);
                srcBuilder.AppendLine($"{outputParamName}=ValueOutput<{returnType.Name}>(nameof({outputParamName}));");
                srcBuilder.AppendLine();
            }

            // Definition - Requirements
            srcBuilder.AppendLine("\t// Requirements");
            foreach (var param in parameters)
            {
                if (!param.HasDefaultValue)
                {
                    var inputParamName = GetInputParamName(param.Name);
                    srcBuilder.AppendLine($"Requirement({inputParamName}, {_INPUT_TRIGGER_NAME});");
                }
            }

            srcBuilder.AppendLine();

            // Definition - Successions
            srcBuilder.AppendLine("\t// Successions");
            srcBuilder.AppendLine($"Succession({_INPUT_TRIGGER_NAME}, {_OUTPUT_TRIGGER_NAME});");
            srcBuilder.AppendLine();

            // TODO: Definition - Assignments
            if (returnType != typeof(void))
            {
                srcBuilder.AppendLine("\t// Assignments");
                var outputParamName = GetOutputParamName(returnType.Name);
                srcBuilder.AppendLine($"Assignment({_INPUT_TRIGGER_NAME}, {outputParamName});");
                srcBuilder.AppendLine();
            }

            // Definition method end
            srcBuilder.AppendLine("}");
            srcBuilder.AppendLine();

            // TODO: OnTriggered method start
            srcBuilder.AppendLine("private ControlOutput OnTriggered(Flow flow)").AppendLine("{");

            // OnTriggered - Input param values
            srcBuilder.AppendLine("\t// Input parameter values");
            foreach (var param in parameters)
            {
                var inputParamName = GetInputParamName(param.Name);
                var inputParamType = param.ParameterType.Name;
                var inputValueName = GetInputValueName(param.Name);
                srcBuilder.AppendLine($"var {inputValueName} = flow.GetValue<{inputParamType}>({inputParamName});");
            }

            srcBuilder.AppendLine();

            // OnTriggered - Output param values
            if (returnType != typeof(void))
            {
                srcBuilder.AppendLine("\t// Output parameter values");

                var outputValueName = GetOutputValueName(returnType.Name);
                var callMethodCode = GenerateCallMethodCode(type.Name, method, parameters);
                srcBuilder.AppendLine($"var {outputValueName} = {callMethodCode}");

                var outputParamName = GetOutputParamName(returnType.Name);
                srcBuilder.AppendLine($"flow.SetValue({outputParamName}, {outputValueName});");
            }

            // OnTriggered - Return
            srcBuilder.AppendLine($"return {_OUTPUT_TRIGGER_NAME};");

            // OnTriggered method end
            srcBuilder.AppendLine("}");
            srcBuilder.AppendLine();

            // Class end and namespace end
            srcBuilder.AppendLine("}").AppendLine("}").AppendLine();

            return srcBuilder.ToString();
        }


        private static void CollectUsingNamespace(MethodBase method, HashSet<string> usingSet)
        {
            var parameters = method.GetParameters();
            foreach (var param in parameters)
            {
                var paramNs = param.ParameterType.Namespace;
                usingSet.Add($"using {paramNs};");
            }
        }

        private static string GetInputParamName(string name) => $"Input_{name}";

        private static string GetInputValueName(string name) => $"Value_{GetInputParamName(name)}";

        private static string GetOutputParamName(string name) => $"Output_{name}";

        private static string GetOutputValueName(string name) => $"Value_{GetOutputParamName(name)}";

        private static string GenerateCallMethodCode(string typeName, MethodBase method, ParameterInfo[] parameters)
        {
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

            if (method is MethodInfo)
            {
                if (method.IsStatic)
                {
                    return $"{typeName}.{method.Name}({paramBuilder.ToString()});";
                }

                var instanceName = GetInputParamName(typeName);
                return $"{instanceName}.{method.Name}({paramBuilder.ToString()});";
            }

            // Ctor
            return $"new {typeName}({paramBuilder.ToString()});";
        }
    }
}