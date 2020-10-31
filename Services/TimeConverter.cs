using System;
using System.Text.RegularExpressions;

namespace facebook_messages_analyser.Services {
    public static class TimeConverter{
        public static DateTime MillisecondsToDateTime(long ms){
            DateTime result = new DateTime(1970,1,1).AddMilliseconds(ms);
            return result;
        }
    }
}