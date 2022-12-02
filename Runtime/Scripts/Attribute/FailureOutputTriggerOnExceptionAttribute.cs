using System;

namespace GBG.VisualPlayable.Attribute
{
    [AttributeUsage(AttributeTargets.Method)]
    internal abstract class FailureOutputTriggerAttributeBase : System.Attribute
    {
        public string SuccessName { get; set; } = "Success";

        public string FailureName { get; set; } = "Failure";
    }

    internal class FailureOutputTriggerOnExceptionAttribute : FailureOutputTriggerAttributeBase
    {
    }

    internal class FailureOutputTriggerOnReturnNullAttribute : FailureOutputTriggerAttributeBase
    {
    }

    internal class FailureOutputTriggerOnReturnFalseAttribute : FailureOutputTriggerAttributeBase
    {
    }

    internal class FailureOutputTriggerOnReturnMinusAttribute : FailureOutputTriggerAttributeBase
    {
    }
}