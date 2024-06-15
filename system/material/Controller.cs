using System.ComponentModel;

namespace Butterfly
{
    public abstract class Controller : system.objects.main.Object
    {
        public Controller() : base(system.objects.main.information.Header.Data.CONTROLLER) { }

        public abstract class LocalField<F> : Controller, ILocalField
        {
            public LocalField() : base() { }

            protected F Field;

            void ILocalField.SetField(object value)
            {
                if (value is F valueReduse)
                {
                    Field = valueReduse;
                }
                else
                    Exception($"При создании обьекта вы передали локальное значение типа {value.GetType()}" +
                              $", но при создании обьекта указали ожидаемый тип {typeof(F).FullName}");
            }
        }

        public abstract class Output<T> : system.objects.main.Object, system.objects.main.IRedirect<T>
        {
            public Output() : base(system.objects.main.information.Header.Data.CONTROLLER) { }

            private IInput<T> _input;

            protected void output(T value) => _input.To(value);

            void system.objects.main.IRedirect<T>.output_to(System.Action<T> action)
                => input_to(ref _input, action);

            system.objects.main.IRedirect<R> system.objects.main.IRedirect<T>.output_to<R>(System.Func<T, R> func)
                => input_to<T, R>(ref _input, func);

            void system.objects.main.IRedirect<T>.send_message_to(string name)
                => send_message(ref _input, name);

            system.objects.main.IRedirect system.objects.main.IRedirect<T>.send_echo_to(string name)
                => send_echo_1_0<T>(ref _input, name);

            void system.objects.main.IRedirect<T>.output_to(System.Action<T> action, string name)
                => input_to(ref _input, name, action);

            void system.objects.main.IRedirect<T>.output_to(System.Action<T> action, string name, System.Action<T> safe)
                => input_to(ref _input, name, action, safe);

            system.objects.main.IRedirect<R> system.objects.main.IRedirect<T>.output_to<R>(System.Func<T, R> func, string name)
                => input_to(ref _input, name, func, null);

            system.objects.main.IRedirect<R> system.objects.main.IRedirect<T>.output_to<R>(System.Func<T, R> func, string name, System.Action<T> safe)
                => input_to(ref _input, name, func, safe);

            void system.objects.main.IRedirect<T>.safe_send_message(string name)
                => safe_send_message(ref _input, name);

            system.objects.main.IRedirect<R> system.objects.main.IRedirect<T>.send_echo_to<R>(string name)
                => send_echo_1_1<T, R>(ref _input, name);

            system.objects.main.IRedirect<R1, R2> system.objects.main.IRedirect<T>.send_echo_to<R1, R2>(string name)
                => send_echo_1_2<T, R1, R2>(ref _input, name);

            system.objects.main.IRedirect<R1, R2, R3> system.objects.main.IRedirect<T>.send_echo_to<R1, R2, R3>(string name)
                => send_echo_1_3<T, R1, R2, R3>(ref _input, name);

            system.objects.main.IRedirect<R1, R2, R3, R4> system.objects.main.IRedirect<T>.send_echo_to<R1, R2, R3, R4>(string name)
                => send_echo_1_4<T, R1, R2, R3, R4>(ref _input, name);

            system.objects.main.IRedirect<R1, R2, R3, R4, R5> system.objects.main.IRedirect<T>.send_echo_to<R1, R2, R3, R4, R5>(string name)
                => send_echo_1_5<T, R1, R2, R3, R4, R5>(ref _input, name);

            system.objects.main.IRedirect<R> system.objects.main.IRedirect<T>.output_to<R>(System.Action<T, IReturn<R>> action)
                => input_to_1_1<T, R>(ref _input, action);

            system.objects.main.IRedirect<R1, R2> system.objects.main.IRedirect<T>.output_to<R1, R2>(System.Action<T, IReturn<R1, R2>> action)
                => input_to_1_2<T, R1, R2>(ref _input, action);

            system.objects.main.IRedirect<R1, R2, R3> system.objects.main.IRedirect<T>.output_to<R1, R2, R3>(System.Action<T, IReturn<R1, R2, R3>> action)
                => input_to_1_3<T, R1, R2, R3>(ref _input, action);

            system.objects.main.IRedirect<R1, R2, R3, R4> system.objects.main.IRedirect<T>.output_to<R1, R2, R3, R4>(System.Action<T, IReturn<R1, R2, R3, R4>> action)
                => input_to_1_4<T, R1, R2, R3, R4>(ref _input, action);

            system.objects.main.IRedirect<R1, R2, R3, R4, R5> system.objects.main.IRedirect<T>.output_to<R1, R2, R3, R4, R5>(System.Action<T, IReturn<R1, R2, R3, R4, R5>> action)
                => input_to_1_5<T, R1, R2, R3, R4, R5>(ref _input, action);

            system.objects.main.IRedirect<R> system.objects.main.IRedirect<T>.output_to<R>(System.Action<T, IReturn<R>> action, string name)
                => input_to_1_1<T, R>(ref _input, name, action, null);

            system.objects.main.IRedirect<R1, R2> system.objects.main.IRedirect<T>.output_to<R1, R2>(System.Action<T, IReturn<R1, R2>> action, string name)
                => input_to_1_2<T, R1, R2>(ref _input, name, action, null);

            system.objects.main.IRedirect<R1, R2, R3> system.objects.main.IRedirect<T>.output_to<R1, R2, R3>(System.Action<T, IReturn<R1, R2, R3>> action, string name)
                => input_to_1_3<T, R1, R2, R3>(ref _input, name, action, null);

            system.objects.main.IRedirect<R1, R2, R3, R4> system.objects.main.IRedirect<T>.output_to<R1, R2, R3, R4>(System.Action<T, IReturn<R1, R2, R3, R4>> action, string name)
                => input_to_1_4<T, R1, R2, R3, R4>(ref _input, name, action, null);

            system.objects.main.IRedirect<R1, R2, R3, R4, R5> system.objects.main.IRedirect<T>.output_to<R1, R2, R3, R4, R5>(System.Action<T, IReturn<R1, R2, R3, R4, R5>> action, string name)
                => input_to_1_5<T, R1, R2, R3, R4, R5>(ref _input, name, action, null);

            system.objects.main.IRedirect<R> system.objects.main.IRedirect<T>.output_to<R>(System.Action<T, IReturn<R>> action, string name, System.Action<T> safe)
                => input_to_1_1<T, R>(ref _input, name, action, safe);

            system.objects.main.IRedirect<R1, R2> system.objects.main.IRedirect<T>.output_to<R1, R2>(System.Action<T, IReturn<R1, R2>> action, string name, System.Action<T> safe)
                => input_to_1_2<T, R1, R2>(ref _input, name, action, safe);

            system.objects.main.IRedirect<R1, R2, R3> system.objects.main.IRedirect<T>.output_to<R1, R2, R3>(System.Action<T, IReturn<R1, R2, R3>> action, string name, System.Action<T> safe)
                => input_to_1_3<T, R1, R2, R3>(ref _input, name, action, safe);

            system.objects.main.IRedirect<R1, R2, R3, R4> system.objects.main.IRedirect<T>.output_to<R1, R2, R3, R4>(System.Action<T, IReturn<R1, R2, R3, R4>> action, string name, System.Action<T> safe)
                => input_to_1_4<T, R1, R2, R3, R4>(ref _input, name, action, safe);

            system.objects.main.IRedirect<R1, R2, R3, R4, R5> system.objects.main.IRedirect<T>.output_to<R1, R2, R3, R4, R5>(System.Action<T, IReturn<R1, R2, R3, R4, R5>> action, string name, System.Action<T> safe)
                => input_to_1_5<T, R1, R2, R3, R4, R5>(ref _input, name, action, safe);
        }

        public abstract class Board : system.objects.main.Object
        {
            public Board() : base(system.objects.main.information.Header.Data.BOARD) { }

            public abstract class LocalField<F> : system.objects.main.Object, ILocalField
            {
                public LocalField() : base(system.objects.main.information.Header.Data.BOARD) { }

                protected F Field;

                void ILocalField.SetField(object value)
                {
                    if (value is F valueReduse)
                    {
                        Field = valueReduse;
                    }
                    else
                        Exception($"При создании обьекта вы передали локальное значение типа {value.GetType()}" +
                                  $", но при создании обьекта указали ожидаемый тип {typeof(F).FullName}");
                }

                public abstract class Output<T> : LocalField<F>, system.objects.main.IRedirect<T>
                {
                    public Output() : base() { }

                    private IInput<T> _input;

                    void system.objects.main.IRedirect<T>.output_to(System.Action<T> action)
                       => input_to(ref _input, action);

                    system.objects.main.IRedirect<R> system.objects.main.IRedirect<T>.output_to<R>(System.Func<T, R> func)
                        => input_to<T, R>(ref _input, func);

                    void system.objects.main.IRedirect<T>.send_message_to(string name)
                        => send_message(ref _input, name);

                    system.objects.main.IRedirect system.objects.main.IRedirect<T>.send_echo_to(string name)
                        => send_echo_1_0<T>(ref _input, name);

                    void system.objects.main.IRedirect<T>.output_to(System.Action<T> action, string name)
                        => input_to(ref _input, name, action);

                    void system.objects.main.IRedirect<T>.output_to(System.Action<T> action, string name, System.Action<T> safe)
                        => input_to(ref _input, name, action, safe);

                    system.objects.main.IRedirect<R> system.objects.main.IRedirect<T>.output_to<R>(System.Func<T, R> func, string name)
                        => input_to(ref _input, name, func, null);

                    system.objects.main.IRedirect<R> system.objects.main.IRedirect<T>.output_to<R>(System.Func<T, R> func, string name, System.Action<T> safe)
                        => input_to(ref _input, name, func, safe);

                    void system.objects.main.IRedirect<T>.safe_send_message(string name)
                        => safe_send_message(ref _input, name);

                    system.objects.main.IRedirect<R> system.objects.main.IRedirect<T>.send_echo_to<R>(string name)
                        => send_echo_1_1<T, R>(ref _input, name);

                    system.objects.main.IRedirect<R1, R2> system.objects.main.IRedirect<T>.send_echo_to<R1, R2>(string name)
                        => send_echo_1_2<T, R1, R2>(ref _input, name);

                    system.objects.main.IRedirect<R1, R2, R3> system.objects.main.IRedirect<T>.send_echo_to<R1, R2, R3>(string name)
                        => send_echo_1_3<T, R1, R2, R3>(ref _input, name);

                    system.objects.main.IRedirect<R1, R2, R3, R4> system.objects.main.IRedirect<T>.send_echo_to<R1, R2, R3, R4>(string name)
                        => send_echo_1_4<T, R1, R2, R3, R4>(ref _input, name);

                    system.objects.main.IRedirect<R1, R2, R3, R4, R5> system.objects.main.IRedirect<T>.send_echo_to<R1, R2, R3, R4, R5>(string name)
                        => send_echo_1_5<T, R1, R2, R3, R4, R5>(ref _input, name);

                    system.objects.main.IRedirect<R> system.objects.main.IRedirect<T>.output_to<R>(System.Action<T, IReturn<R>> action)
                        => input_to_1_1<T, R>(ref _input, action);

                    system.objects.main.IRedirect<R1, R2> system.objects.main.IRedirect<T>.output_to<R1, R2>(System.Action<T, IReturn<R1, R2>> action)
                        => input_to_1_2<T, R1, R2>(ref _input, action);

                    system.objects.main.IRedirect<R1, R2, R3> system.objects.main.IRedirect<T>.output_to<R1, R2, R3>(System.Action<T, IReturn<R1, R2, R3>> action)
                        => input_to_1_3<T, R1, R2, R3>(ref _input, action);

                    system.objects.main.IRedirect<R1, R2, R3, R4> system.objects.main.IRedirect<T>.output_to<R1, R2, R3, R4>(System.Action<T, IReturn<R1, R2, R3, R4>> action)
                        => input_to_1_4<T, R1, R2, R3, R4>(ref _input, action);

                    system.objects.main.IRedirect<R1, R2, R3, R4, R5> system.objects.main.IRedirect<T>.output_to<R1, R2, R3, R4, R5>(System.Action<T, IReturn<R1, R2, R3, R4, R5>> action)
                        => input_to_1_5<T, R1, R2, R3, R4, R5>(ref _input, action);

                    system.objects.main.IRedirect<R> system.objects.main.IRedirect<T>.output_to<R>(System.Action<T, IReturn<R>> action, string name)
                        => input_to_1_1<T, R>(ref _input, name, action, null);

                    system.objects.main.IRedirect<R1, R2> system.objects.main.IRedirect<T>.output_to<R1, R2>(System.Action<T, IReturn<R1, R2>> action, string name)
                        => input_to_1_2<T, R1, R2>(ref _input, name, action, null);

                    system.objects.main.IRedirect<R1, R2, R3> system.objects.main.IRedirect<T>.output_to<R1, R2, R3>(System.Action<T, IReturn<R1, R2, R3>> action, string name)
                        => input_to_1_3<T, R1, R2, R3>(ref _input, name, action, null);

                    system.objects.main.IRedirect<R1, R2, R3, R4> system.objects.main.IRedirect<T>.output_to<R1, R2, R3, R4>(System.Action<T, IReturn<R1, R2, R3, R4>> action, string name)
                        => input_to_1_4<T, R1, R2, R3, R4>(ref _input, name, action, null);

                    system.objects.main.IRedirect<R1, R2, R3, R4, R5> system.objects.main.IRedirect<T>.output_to<R1, R2, R3, R4, R5>(System.Action<T, IReturn<R1, R2, R3, R4, R5>> action, string name)
                        => input_to_1_5<T, R1, R2, R3, R4, R5>(ref _input, name, action, null);

                    system.objects.main.IRedirect<R> system.objects.main.IRedirect<T>.output_to<R>(System.Action<T, IReturn<R>> action, string name, System.Action<T> safe)
                        => input_to_1_1<T, R>(ref _input, name, action, safe);

                    system.objects.main.IRedirect<R1, R2> system.objects.main.IRedirect<T>.output_to<R1, R2>(System.Action<T, IReturn<R1, R2>> action, string name, System.Action<T> safe)
                        => input_to_1_2<T, R1, R2>(ref _input, name, action, safe);

                    system.objects.main.IRedirect<R1, R2, R3> system.objects.main.IRedirect<T>.output_to<R1, R2, R3>(System.Action<T, IReturn<R1, R2, R3>> action, string name, System.Action<T> safe)
                        => input_to_1_3<T, R1, R2, R3>(ref _input, name, action, safe);

                    system.objects.main.IRedirect<R1, R2, R3, R4> system.objects.main.IRedirect<T>.output_to<R1, R2, R3, R4>(System.Action<T, IReturn<R1, R2, R3, R4>> action, string name, System.Action<T> safe)
                        => input_to_1_4<T, R1, R2, R3, R4>(ref _input, name, action, safe);

                    system.objects.main.IRedirect<R1, R2, R3, R4, R5> system.objects.main.IRedirect<T>.output_to<R1, R2, R3, R4, R5>(System.Action<T, IReturn<R1, R2, R3, R4, R5>> action, string name, System.Action<T> safe)
                        => input_to_1_5<T, R1, R2, R3, R4, R5>(ref _input, name, action, safe); protected void output(T value) => _input.To(value);

                }
            }

            public class Output<T> : system.objects.main.Object, system.objects.main.IRedirect<T>
            {
                public Output() : base(system.objects.main.information.Header.Data.BOARD) { }

                private IInput<T> _input;

                protected void output(T value) => _input.To(value);

                void system.objects.main.IRedirect<T>.output_to(System.Action<T> action)
                    => input_to(ref _input, action);

                system.objects.main.IRedirect<R> system.objects.main.IRedirect<T>.output_to<R>(System.Func<T, R> func)
                    => input_to<T, R>(ref _input, func);

                void system.objects.main.IRedirect<T>.send_message_to(string name)
                    => send_message(ref _input, name);

                system.objects.main.IRedirect system.objects.main.IRedirect<T>.send_echo_to(string name)
                    => send_echo_1_0<T>(ref _input, name);

                void system.objects.main.IRedirect<T>.output_to(System.Action<T> action, string name)
                    => input_to(ref _input, name, action);

                void system.objects.main.IRedirect<T>.output_to(System.Action<T> action, string name, System.Action<T> safe)
                    => input_to(ref _input, name, action, safe);

                system.objects.main.IRedirect<R> system.objects.main.IRedirect<T>.output_to<R>(System.Func<T, R> func, string name)
                    => input_to(ref _input, name, func, null);

                system.objects.main.IRedirect<R> system.objects.main.IRedirect<T>.output_to<R>(System.Func<T, R> func, string name, System.Action<T> safe)
                    => input_to(ref _input, name, func, safe);

                void system.objects.main.IRedirect<T>.safe_send_message(string name)
                    => safe_send_message(ref _input, name);

                system.objects.main.IRedirect<R> system.objects.main.IRedirect<T>.send_echo_to<R>(string name)
                    => send_echo_1_1<T, R>(ref _input, name);

                system.objects.main.IRedirect<R1, R2> system.objects.main.IRedirect<T>.send_echo_to<R1, R2>(string name)
                    => send_echo_1_2<T, R1, R2>(ref _input, name);

                system.objects.main.IRedirect<R1, R2, R3> system.objects.main.IRedirect<T>.send_echo_to<R1, R2, R3>(string name)
                    => send_echo_1_3<T, R1, R2, R3>(ref _input, name);

                system.objects.main.IRedirect<R1, R2, R3, R4> system.objects.main.IRedirect<T>.send_echo_to<R1, R2, R3, R4>(string name)
                    => send_echo_1_4<T, R1, R2, R3, R4>(ref _input, name);

                system.objects.main.IRedirect<R1, R2, R3, R4, R5> system.objects.main.IRedirect<T>.send_echo_to<R1, R2, R3, R4, R5>(string name)
                    => send_echo_1_5<T, R1, R2, R3, R4, R5>(ref _input, name);

                system.objects.main.IRedirect<R> system.objects.main.IRedirect<T>.output_to<R>(System.Action<T, IReturn<R>> action)
                    => input_to_1_1<T, R>(ref _input, action);

                system.objects.main.IRedirect<R1, R2> system.objects.main.IRedirect<T>.output_to<R1, R2>(System.Action<T, IReturn<R1, R2>> action)
                    => input_to_1_2<T, R1, R2>(ref _input, action);

                system.objects.main.IRedirect<R1, R2, R3> system.objects.main.IRedirect<T>.output_to<R1, R2, R3>(System.Action<T, IReturn<R1, R2, R3>> action)
                    => input_to_1_3<T, R1, R2, R3>(ref _input, action);

                system.objects.main.IRedirect<R1, R2, R3, R4> system.objects.main.IRedirect<T>.output_to<R1, R2, R3, R4>(System.Action<T, IReturn<R1, R2, R3, R4>> action)
                    => input_to_1_4<T, R1, R2, R3, R4>(ref _input, action);

                system.objects.main.IRedirect<R1, R2, R3, R4, R5> system.objects.main.IRedirect<T>.output_to<R1, R2, R3, R4, R5>(System.Action<T, IReturn<R1, R2, R3, R4, R5>> action)
                    => input_to_1_5<T, R1, R2, R3, R4, R5>(ref _input, action);

                system.objects.main.IRedirect<R> system.objects.main.IRedirect<T>.output_to<R>(System.Action<T, IReturn<R>> action, string name)
                    => input_to_1_1<T, R>(ref _input, name, action, null);

                system.objects.main.IRedirect<R1, R2> system.objects.main.IRedirect<T>.output_to<R1, R2>(System.Action<T, IReturn<R1, R2>> action, string name)
                    => input_to_1_2<T, R1, R2>(ref _input, name, action, null);

                system.objects.main.IRedirect<R1, R2, R3> system.objects.main.IRedirect<T>.output_to<R1, R2, R3>(System.Action<T, IReturn<R1, R2, R3>> action, string name)
                    => input_to_1_3<T, R1, R2, R3>(ref _input, name, action, null);

                system.objects.main.IRedirect<R1, R2, R3, R4> system.objects.main.IRedirect<T>.output_to<R1, R2, R3, R4>(System.Action<T, IReturn<R1, R2, R3, R4>> action, string name)
                    => input_to_1_4<T, R1, R2, R3, R4>(ref _input, name, action, null);

                system.objects.main.IRedirect<R1, R2, R3, R4, R5> system.objects.main.IRedirect<T>.output_to<R1, R2, R3, R4, R5>(System.Action<T, IReturn<R1, R2, R3, R4, R5>> action, string name)
                    => input_to_1_5<T, R1, R2, R3, R4, R5>(ref _input, name, action, null);

                system.objects.main.IRedirect<R> system.objects.main.IRedirect<T>.output_to<R>(System.Action<T, IReturn<R>> action, string name, System.Action<T> safe)
                    => input_to_1_1<T, R>(ref _input, name, action, safe);

                system.objects.main.IRedirect<R1, R2> system.objects.main.IRedirect<T>.output_to<R1, R2>(System.Action<T, IReturn<R1, R2>> action, string name, System.Action<T> safe)
                    => input_to_1_2<T, R1, R2>(ref _input, name, action, safe);

                system.objects.main.IRedirect<R1, R2, R3> system.objects.main.IRedirect<T>.output_to<R1, R2, R3>(System.Action<T, IReturn<R1, R2, R3>> action, string name, System.Action<T> safe)
                    => input_to_1_3<T, R1, R2, R3>(ref _input, name, action, safe);

                system.objects.main.IRedirect<R1, R2, R3, R4> system.objects.main.IRedirect<T>.output_to<R1, R2, R3, R4>(System.Action<T, IReturn<R1, R2, R3, R4>> action, string name, System.Action<T> safe)
                    => input_to_1_4<T, R1, R2, R3, R4>(ref _input, name, action, safe);

                system.objects.main.IRedirect<R1, R2, R3, R4, R5> system.objects.main.IRedirect<T>.output_to<R1, R2, R3, R4, R5>(System.Action<T, IReturn<R1, R2, R3, R4, R5>> action, string name, System.Action<T> safe)
                    => input_to_1_5<T, R1, R2, R3, R4, R5>(ref _input, name, action, safe);
            }
        }
    }

    public interface ILocalField
    {
        public void SetField(object value);
    }
}