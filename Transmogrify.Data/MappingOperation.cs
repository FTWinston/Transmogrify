using Newtonsoft.Json;
using System;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Transmogrify.Data.Serialization;

namespace Transmogrify.Data
{
    [JsonObject(IsReference = true)]
    public class MappingOperation : MappingElement
    {
        public MappingOperation(MethodInfo method)
        {
            Method = method;

            var parameters = method.GetParameters();

            var inputParams = parameters.Where(p => p.IsIn);

            var outputParams = parameters
                .Where(p => p.IsOut)
                .Select(p => new DataField(p.Name, p.ParameterType))
                .ToList();

            if (method.ReturnType != typeof(void))
            {
                outputParams.Insert(0, new DataField("Value", method.ReturnType));
            }

            RawInputs = new DataFieldInstance[inputParams.Count()];

            RawOutputs = outputParams.Select(o => new MappingOperationOutputFieldInstance(this, o))
                .ToArray();
        }

        [JsonConstructor]
        private MappingOperation(Type methodType, string methodIdentifier)
        {
            Method = SerializationExtensions.GetMethod(methodType, methodIdentifier);

            if (Method == null)
                throw new Exception("Couldn't find method");
        }

        [JsonIgnore]
        public MethodInfo Method { get; }

        [JsonProperty(PropertyName = "Type")]
        private Type MethodType => Method.DeclaringType;

        [JsonProperty(PropertyName = "Method")]
        private string MethodIdentifier => Method.GetUniqueIdentifier();

        [JsonIgnore]
        public DataFieldInstance[] Inputs => RawInputs; // These point at another element's fields

        [JsonProperty(PropertyName = "Inputs")]
        private DataFieldInstance[] RawInputs { get; set; }

        [JsonIgnore]
        public DataFieldInstance[] Outputs => RawOutputs; // These point at this element's fields

        [JsonProperty(PropertyName = "Outputs")]
        private MappingOperationOutputFieldInstance[] RawOutputs { get; set; }

        public int X { get; set; }
        public int Y { get; set; }

        [OnDeserialized]
        internal void OnDeserialized(StreamingContext context)
        {
            // Once deserialized, set set the element of every output to this.
            // These weren't serialized, as this is a "parent" of its outputs in the JSON.

            if (RawOutputs == null)
                return;

            foreach (var output in RawOutputs)
                if (output != null)
                    output.Element = this;
        }
    }
}
