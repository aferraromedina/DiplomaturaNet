namespace EjemploEntradaSalida
{
    public partial class Mate
    {
        public string GetDescripcion()
        {
            return $"Soy un mate de {Color} y mi dueño es {Propietario}";
        }
        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            var otroMate = (Mate)obj;

            return Color == otroMate.Color &&
                Propietario == otroMate.Propietario &&
                ((CaracteristicasEspeciales == null && otroMate.CaracteristicasEspeciales == null) ||
                CaracteristicasEspeciales.SequenceEqual(otroMate.CaracteristicasEspeciales));
        }
        public override int GetHashCode()
        {
            return Propietario != null ? Propietario.GetHashCode() : base.GetHashCode();
        }
    }
}
