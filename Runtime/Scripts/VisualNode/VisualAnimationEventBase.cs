using Unity.VisualScripting;

namespace GBG.VisualPlayable.VisualNode
{
    public readonly struct VisualAnimationEventArgs
    {
        public readonly string LayerName;

        public readonly byte LayerIndex;

        public readonly string MainAnimationTag;


        public VisualAnimationEventArgs(string layerName, byte layerIndex, string mainAnimationTag)
        {
            LayerName = layerName;
            LayerIndex = layerIndex;
            MainAnimationTag = mainAnimationTag;
        }
    }

    public abstract class VisualAnimationEventBase : EventUnit<VisualAnimationEventArgs>
    {
        [PortLabel("Animation Layer Name")]
        [DoNotSerialize]
        public ValueOutput OutputLayerName { get; private set; }

        [PortLabel("Animation Layer Index")]
        [DoNotSerialize]
        public ValueOutput OutputLayerIndex { get; private set; }

        [PortLabel("Main Animation Tag")]
        [DoNotSerialize]
        public ValueOutput OutputMainAnimationTag { get; private set; }

        protected abstract string EventHookName { get; }

        protected override bool register => true;


        public override EventHook GetHook(GraphReference reference)
        {
            return new EventHook(EventHookName);
        }

        protected override void Definition()
        {
            base.Definition();

            OutputLayerName = ValueOutput<string>(nameof(OutputLayerName));
            OutputLayerIndex = ValueOutput<byte>(nameof(OutputLayerIndex));
            OutputMainAnimationTag = ValueOutput<string>(nameof(OutputMainAnimationTag));
        }

        protected override void AssignArguments(Flow flow, VisualAnimationEventArgs args)
        {
            flow.SetValue(OutputLayerName, args.LayerName);
            flow.SetValue(OutputLayerIndex, args.LayerIndex);
            flow.SetValue(OutputMainAnimationTag, args.MainAnimationTag);
        }
    }
}