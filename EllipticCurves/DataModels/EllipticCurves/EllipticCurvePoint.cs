using EllipticCurves.DataModels.FiniteFields;

namespace EllipticCurves.DataModels.EllipticCurves
{
    public class EllipticCurvePoint
    {
        public FiniteFieldValue X { get; }
        public FiniteFieldValue Y { get; }
        public bool IsInfinity { get; }

        public static EllipticCurvePoint Infinity { get; } = new EllipticCurvePoint(isInfinity: true);

        public EllipticCurvePoint(FiniteFieldValue x, FiniteFieldValue y)
        {
            X = x;
            Y = y;
        }
        
        private EllipticCurvePoint(bool isInfinity)
        {
            IsInfinity = isInfinity;
        }
        
        public override bool Equals(object obj)
        {
            if (!(obj is EllipticCurvePoint point)) 
                return false;
            
            if (IsInfinity && point.IsInfinity)
                return true;
            
            return X == point.X && Y == point.Y;
        }

        public override string ToString()
        {
            return IsInfinity ? "[ECP: ∞]" : $"[ECP: x={X}, y={Y}]";
        }
        
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (X != null ? X.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Y != null ? Y.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ IsInfinity.GetHashCode();
                return hashCode;
            }
        }
    }
}