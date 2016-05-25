using System;
using System.Collections.Generic;
using System.Text;

namespace SQLComparator
{
    public class MatchDetail : IComparable
    {

        #region Enumerations

        public enum MatchType
        {
            None, None1, None2, Partial, Full
        }

        #endregion Enumerations

        #region Static

        static int _NoMatchCount = 0;
        static int _FullMatchCount = 0;
        static int _PartialMatchCount = 0;
        static int _None1 = 0;
        static int _None2 = 0;

        public static void Reset()
        {
            _NoMatchCount = 0;
            _FullMatchCount = 0;
            _PartialMatchCount = 0;
            _None1 = 0;
            _None2 = 0;

        }

        public static int GetTypeCount(MatchType type)
        {
            switch (type)
            {
                case MatchType.Full:
                    return _FullMatchCount;
                    break;

                case MatchType.Partial:
                    return _PartialMatchCount;
                    break;

                case MatchType.None:
                    return _NoMatchCount;
                    break;

                case MatchType.None1:
                    return _None1;
                    break;

                case MatchType.None2:
                    return _None2;
                    break;

                default:
                    throw new Exception("Unhandled MatchType");
                    return 0;
            }
        }

        public static MatchDetail GetMatchDetail(MatchType type)
        {
            return new MatchDetail(type);
        }

        #endregion Static

        #region Constructor

        private MatchDetail() { }

        private MatchDetail(MatchType type)
        {
            this._MatchType = type;
            switch (type)
            {
                case MatchType.Full:
                    _InstanceCount = ++_FullMatchCount;
                    break;

                case MatchType.Partial:
                    _InstanceCount = ++_PartialMatchCount;
                    break;

                case MatchType.None:
                    _InstanceCount = ++_NoMatchCount;
                    break;

                case MatchType.None1:
                    _InstanceCount = ++_None1;
                    break;

                case MatchType.None2:
                    _InstanceCount = ++_None2;
                    break;


            }
        }

        #endregion

        #region Instance variables

        private MatchType _MatchType;
        private int _InstanceCount;

        #endregion

        #region Instance Public

        public override string ToString()
        {

            switch (this._MatchType)
            {


                case MatchType.None:
                    return _MatchType.ToString(); 
                    break;

                case MatchType.None2:
                       return ">>>>>>";  return "<<<<<<";
                 
                   // return "↓↓↓↓↓↓";
     
                  
                    return ">>>>>>";
                    break;

                case MatchType.None1:
                    return "<<<<<<";  return ">>>>>>";
                 
                //    return "↑↑↑↑↑↑";
                    return "<<<<<<";
                    break;

                default:

                    return _MatchType.ToString() + " " + _InstanceCount.ToString();
            }


          

        }
        public MatchType Type { get { return _MatchType; } }
        public int InstanceCount { get { return _InstanceCount; } }

        #endregion

        #region IComparable Members

        public int CompareTo(object obj)
        {

            if (!(obj is MatchDetail))
                throw new ArgumentException("Object to compare to must be a MatchDetail");

            MatchDetail md = obj as MatchDetail;
            return (md._MatchType == this._MatchType ? this._InstanceCount - md._InstanceCount : md._MatchType - this._MatchType);

        }

        #endregion
    }

}
