namespace Transmogrify.Engine
{
    class MethodData
    {
        public delegate object CallDelegate(object target, object[] paramters);
        
        public CallDelegate Delegate { get; set; }

        public bool OutputReturnType { get; set; }

        public bool[] InputParameters { get; set; }

        public bool[] OutputParameters { get; set; }

        public int NumOutputs { get; set; }
    }
}
