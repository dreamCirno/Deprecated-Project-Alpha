using GameFramework.Fsm;
using GameFramework.Procedure;
using ProcedureBase = GameMain.Procedure.Base.ProcedureBase;

namespace GameMain.Procedure.Impl {
    public class ProcedureSplash : ProcedureBase {
        protected override void OnEnter(IFsm<IProcedureManager> procedureOwner) {
            base.OnEnter(procedureOwner);
        }
    }
}