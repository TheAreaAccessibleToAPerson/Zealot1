namespace Butterfly
{
    namespace Return
    {
        public interface IInformation
        {
            string GetKey();
            ulong GetID();
            ulong GetUnieueID();
        }
    }

    public interface IReturn : Return.IInformation
    { void To(); }

    /// <summary>
    /// Описывает способ возрата данных и информацию об отправителе. 
    /// </summary>
    /// <typeparam name="R"></typeparam>
    public interface IReturn<R> : Return.IInformation
    { void To(R value); }
    /// <summary>
    /// Описывает способ возрата данных и информацию об отправителе. 
    /// </summary>
    public interface IReturn<R1, R2> : Return.IInformation
    { void To(R1 value1, R2 value2); }
    /// <summary>
    /// Описывает способ возрата данных и информацию об отправителе. 
    /// </summary>
    public interface IReturn<R1, R2, R3> : Return.IInformation
    { void To(R1 value1, R2 value2, R3 value3); }
    /// <summary>
    /// Описывает способ возрата данных и информацию об отправителе. 
    /// </summary>
    public interface IReturn<R1, R2, R3, R4> : Return.IInformation
    { void To(R1 value1, R2 value2, R3 value3, R4 value4); }
    /// <summary>
    /// Описывает способ возрата данных и информацию об отправителе. 
    /// </summary>
    public interface IReturn<R1, R2, R3, R4, R5> : Return.IInformation
    { void To(R1 value1, R2 value2, R3 value3, R4 value4, R5 value5); }
    /// <summary>
    /// Описывает способ возрата данных и информацию об отправителе. 
    /// </summary>
    public interface IReturn<R1, R2, R3, R4, R5, R6> : Return.IInformation
    { void To(R1 value1, R2 value2, R3 value3, R4 value4, R5 value5, R6 value6); }
    /// <summary>
    /// Описывает способ возрата данных и информацию об отправителе. 
    /// </summary>
    public interface IReturn<R1, R2, R3, R4, R5, R6, R7> : Return.IInformation
    { void To(R1 value1, R2 value2, R3 value3, R4 value4, R5 value5, R6 value6, R7 value7); }
    /// <summary>
    /// Описывает способ возрата данных и информацию об отправителе. 
    /// </summary>
    public interface IReturn<R1, R2, R3, R4, R5, R6, R7, R8> : Return.IInformation
    { void To(R1 value1, R2 value2, R3 value3, R4 value4, R5 value5, R6 value6, R7 value7, R8 value8); }
    /// <summary>
    /// Описывает способ возрата данных и информацию об отправителе. 
    /// </summary>
    public interface IReturn<R1, R2, R3, R4, R5, R6, R7, R8, R9> : Return.IInformation
    { void To(R1 value1, R2 value2, R3 value3, R4 value4, R5 value5, R6 value6, R7 value7, R8 value8, R9 value9); }
    /// <summary>
    /// Описывает способ возрата данных и информацию об отправителе. 
    /// </summary>
    public interface IReturn<R1, R2, R3, R4, R5, R6, R7, R8, R9, R10> : Return.IInformation
    { void To(R1 value1, R2 value2, R3 value3, R4 value4, R5 value5, R6 value6, R7 value7, R8 value8, R9 value9, R10 value10); }
    /// <summary>
    /// Описывает способ возрата данных и информацию об отправителе. 
    /// </summary>
    public interface IReturn<R1, R2, R3, R4, R5, R6, R7, R8, R9, R10, R11> : Return.IInformation
    { void To(R1 value1, R2 value2, R3 value3, R4 value4, R5 value5, R6 value6, R7 value7, R8 value8, R9 value9, R10 value10, R11 value11); }
    /// <summary>
    /// Описывает способ возрата данных и информацию об отправителе. 
    /// </summary>
    public interface IReturn<R1, R2, R3, R4, R5, R6, R7, R8, R9, R10, R11, R12> : Return.IInformation
    { void To(R1 value1, R2 value2, R3 value3, R4 value4, R5 value5, R6 value6, R7 value7, R8 value8, R9 value9, R10 value10, R11 value11, R12 value12); }
    /// <summary>
    /// Описывает способ возрата данных и информацию об отправителе. 
    /// </summary>
    public interface IReturn<R1, R2, R3, R4, R5, R6, R7, R8, R9, R10, R11, R12, R13> : Return.IInformation
    { void To(R1 value1, R2 value2, R3 value3, R4 value4, R5 value5, R6 value6, R7 value7, R8 value8, R9 value9, R10 value10, R11 value11, R12 value12, R13 value13); }
    /// <summary>
    /// Описывает способ возрата данных и информацию об отправителе. 
    /// </summary>
    public interface IReturn<R1, R2, R3, R4, R5, R6, R7, R8, R9, R10, R11, R12, R13, R14> : Return.IInformation
    { void To(R1 value1, R2 value2, R3 value3, R4 value4, R5 value5, R6 value6, R7 value7, R8 value8, R9 value9, R10 value10, R11 value11, R12 value12, R13 value13, R14 value14); }
    /// <summary>
    /// Описывает способ возрата данных и информацию об отправителе. 
    /// </summary>
    public interface IReturn<R1, R2, R3, R4, R5, R6, R7, R8, R9, R10, R11, R12, R13, R14, R15> : Return.IInformation
    { void To(R1 value1, R2 value2, R3 value3, R4 value4, R5 value5, R6 value6, R7 value7, R8 value8, R9 value9, R10 value10, R11 value11, R12 value12, R13 value13, R14 value14, R15 value15); }
    /// <summary>
    /// Описывает способ возрата данных и информацию об отправителе. 
    /// </summary>
    public interface IReturn<R1, R2, R3, R4, R5, R6, R7, R8, R9, R10, R11, R12, R13, R14, R15, R16> : Return.IInformation
    { void To(R1 value1, R2 value2, R3 value3, R4 value4, R5 value5, R6 value6, R7 value7, R8 value8, R9 value9, R10 value10, R11 value11, R12 value12, R13 value13, R14 value14, R15 value15, R16 value16); }
}