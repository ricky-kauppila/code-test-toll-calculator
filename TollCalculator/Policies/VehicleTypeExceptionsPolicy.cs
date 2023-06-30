namespace TollCalculator.Policies;

public class VehicleTypeExceptionsPolicy : FeePolicy
{
    private readonly FeePolicyDelegate next;

    public VehicleTypeExceptionsPolicy(FeePolicyDelegate next)
    {
        this.next = next;
    }

    public override TollFee Apply(Pass pass)
    {
        return IsTollFreeVehicle(pass.Vehicle) ? TollFee.Free(pass) : next(pass);
    }

    private bool IsTollFreeVehicle(IVehicle vehicle)
    {
        var vehicleType = vehicle.GetVehicleType();
        return vehicleType.Equals(TollFreeVehicles.Motorbike.ToString()) ||
               vehicleType.Equals(TollFreeVehicles.Tractor.ToString()) ||
               vehicleType.Equals(TollFreeVehicles.Emergency.ToString()) ||
               vehicleType.Equals(TollFreeVehicles.Diplomat.ToString()) ||
               vehicleType.Equals(TollFreeVehicles.Foreign.ToString()) ||
               vehicleType.Equals(TollFreeVehicles.Military.ToString());
    }
}