using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TDCD_SharingManga.Common
{
    public static class Constants
    {
        public static string USER_SESSION = "USER_SESSION";
        public enum STATUS
        {
            Published,
            UnPublished,
            Deleted
        }
        public static int PAGE_SIZE = 10;
    }
}