using System;

namespace CustomUtility
{
    namespace IO
    {
        /// <summary>
        /// # Summary
        /// 다양한 저장 핸들러를 사용하여 데이터를 저장하고 불러오는 작업을 관리하는 정적 컨트롤러 클래스.
        /// 이 클래스는 지정된 저장 유형에 따라 적절한 저장 핸들러를 선택하는 로직을 추상화한다.
        ///
        /// ## 사용 방법:
        /// - `Save` 메서드를 사용하여 지정된 저장 유형으로 데이터 객체를 저장한다.
        /// - `Load` 메서드를 사용하여 지정된 저장 유형으로 데이터 객체를 불러온다.
        ///
        /// ## 기능:
        /// - 여러 저장 유형을 처리하기 위해 저장 핸들러 인스턴스를 동적으로 관리한다.
        /// - 지정된 저장 유형에 따라 올바른 저장 핸들러가 사용되도록 보장한다.
        /// </summary>
        public static class DataSaveController
        {
            // Array to hold instances of save handlers for different save types
            private static SaveHandle[] _saveHandles = new SaveHandle[Enum.GetValues(typeof(SaveType)).Length];
        
            /// <summary>
            /// 지정된 저장 유형을 사용하여 주어진 데이터 객체를 저장한다.
            /// </summary>
            /// <typeparam name="T">The type of data to save. 저장할 데이터의 타입.</typeparam>
            /// <param name="target">The data object to save. 저장할 데이터 객체.</param>
            /// <param name="type">The save type to use (e.g., JSON, Binary). 사용할 저장 유형(예: JSON, Binary).</param>
            public static void Save<T>(T target, SaveType type) where T : SaveData
            {
                GetSaveHandleInstance(type).Save(target);
            }

            /// <summary>
            /// 지정된 저장 유형을 사용하여 데이터를 제공된 데이터 객체로 불러온다.
            /// </summary>
            /// <typeparam name="T">The type of data to load. 불러올 데이터의 타입.</typeparam>
            /// <param name="target">The data object to load into. 데이터를 불러올 객체.</param>
            /// <param name="type">The save type to use (e.g., JSON, Binary). 사용할 저장 유형(예: JSON, Binary).</param>
            public static void Load<T>(ref T target, SaveType type) where T : SaveData, new()
            {
                GetSaveHandleInstance(type).Load(ref target);
            }

            /// <summary>
            /// 지정된 저장 유형에 따라 저장 핸들러 인스턴스를 가져온다.
            /// </summary>
            /// <param name="type">The save type to use (e.g., JSON, Binary). 사용할 저장 유형(예: JSON, Binary).</param>
            /// <returns>Returns an instance of the appropriate save handler. 적절한 저장 핸들러의 인스턴스를 반환한다.</returns>
            private static SaveHandle GetSaveHandleInstance(SaveType type)
            {
                if (_saveHandles[(int)type] == null)
                {
                    switch (type)
                    {
                        case SaveType.JSON:
                            _saveHandles[(int)type] = new JsonSaveHandler();
                            break;
                        case SaveType.BINARY:
                            _saveHandles[(int)type] = new BinarySaveHandler();
                            break;
                    }
                }

                return _saveHandles[(int)type];
            }
        }
    }
}
