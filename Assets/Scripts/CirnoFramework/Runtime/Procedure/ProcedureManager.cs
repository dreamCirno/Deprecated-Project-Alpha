using System;
using System.Linq;
using System.Reflection;
using CirnoFramework.Runtime.Base;
using CirnoFramework.Runtime.Fsm;
using CirnoFramework.Runtime.Utility;
using GameFramework;
using GameFramework.Fsm;
using GameFramework.Procedure;
using GameFramework.Resource;

namespace CirnoFramework.Runtime.Procedure {
    public class ProcedureManager : IGameFrameworkModule {
        public int Priority => 2;

        public void OnInit() {
            ReadProcedureTypeNames();

            ProcedureBase[] procedures = new ProcedureBase[m_ProcedureTypeNames.Length];
            for (int i = 0; i < m_ProcedureTypeNames.Length; i++) {
                Type procedureType = GameFramework.Utility.Assembly.GetType(m_ProcedureTypeNames[i]);
                if (procedureType == null) {
                    Log.Error("Can not find procedure type '{0}'.", m_ProcedureTypeNames[i]);
                    return;
                }

                procedures[i] = (ProcedureBase) Activator.CreateInstance(procedureType);
                if (procedures[i] == null) {
                    Log.Error("Can not create procedure instance '{0}'.", m_ProcedureTypeNames[i]);
                    return;
                }

                if (procedures[i].GetType().GetCustomAttribute(typeof(ProcedureAttribute)) is ProcedureAttribute {
                    StateType: ProcedureType.Start
                }) {
                    m_EntranceProcedure = procedures[i];
                }
            }

            if (m_EntranceProcedure == null) {
                if (procedures.Length > 0) {
                    m_EntranceProcedure = procedures.First();
                    Log.Warning("ProcedureManager does not found ProcedureType.Start Attribute, " +
                                "it will select a default procedure as entrance procedure.");
                }
                else {
                    Log.Fatal("ProcedureManager does not have a entrance procedure.");
                    return;
                }
            }

            m_ProcedureManager = GameFrameworkEntry.GetModule<IProcedureManager>();
            m_ProcedureManager.Initialize(GameFrameworkEntry.GetModule<IFsmManager>(), procedures);
            m_ProcedureManager.StartProcedure(m_EntranceProcedure.GetType());
        }

        public void OnClose() {
        }

        private IProcedureManager m_ProcedureManager = null;
        private ProcedureBase m_EntranceProcedure = null;

        private string[] m_ProcedureTypeNames = null;

        #region Public interface

        /// <summary>
        /// 获取当前流程。
        /// </summary>
        public ProcedureBase CurrentProcedure {
            get { return m_ProcedureManager.CurrentProcedure; }
        }

        /// <summary>
        /// 获取当前流程持续时间。
        /// </summary>
        public float CurrentProcedureTime {
            get { return m_ProcedureManager.CurrentProcedureTime; }
        }

        /// <summary>
        /// 是否存在流程。
        /// </summary>
        /// <typeparam name="T">要检查的流程类型。</typeparam>
        /// <returns>是否存在流程。</returns>
        public bool HasProcedure<T>() where T : ProcedureBase {
            return m_ProcedureManager.HasProcedure<T>();
        }

        /// <summary>
        /// 是否存在流程。
        /// </summary>
        /// <param name="procedureType">要检查的流程类型。</param>
        /// <returns>是否存在流程。</returns>
        public bool HasProcedure(Type procedureType) {
            return m_ProcedureManager.HasProcedure(procedureType);
        }

        /// <summary>
        /// 获取流程。
        /// </summary>
        /// <typeparam name="T">要获取的流程类型。</typeparam>
        /// <returns>要获取的流程。</returns>
        public ProcedureBase GetProcedure<T>() where T : ProcedureBase {
            return m_ProcedureManager.GetProcedure<T>();
        }

        /// <summary>
        /// 获取流程。
        /// </summary>
        /// <param name="procedureType">要获取的流程类型。</param>
        /// <returns>要获取的流程。</returns>
        public ProcedureBase GetProcedure(Type procedureType) {
            return m_ProcedureManager.GetProcedure(procedureType);
        }

        #endregion

        #region Inner interface

        private void ReadProcedureTypeNames() {
            m_ProcedureTypeNames = TypeUtil.GetRuntimeTypeNames(typeof(ProcedureBase));
        }

        #endregion
    }
}