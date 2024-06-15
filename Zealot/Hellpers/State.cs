namespace Zealot.hellper
{
    public static class State 
    {
        /// <summary>
        /// Принимает на вход следующее состояние и массив состояний на которые
        /// можено перейти. 
        /// </summary>
        /// <param name="currentState"></param>
        /// <param name="nextStates"></param>
        /// <returns></returns>
        public static bool Contains(string nextState, string[] states)
        {
            foreach(string s in states)
                if (nextState == s) return true;

            return false;
        }
    }
}