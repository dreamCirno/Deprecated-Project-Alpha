using System;

namespace CirnoFramework.Runtime.Procedure {
    public class ProcedureAttribute : Attribute {
        public ProcedureType StateType { get; private set; }

        public ProcedureAttribute(ProcedureType stateType = ProcedureType.Normal) {
            stateType = stateType;
        }
    }

    public enum ProcedureType {
        Start,
        Normal,
        OverStart,
    }
}