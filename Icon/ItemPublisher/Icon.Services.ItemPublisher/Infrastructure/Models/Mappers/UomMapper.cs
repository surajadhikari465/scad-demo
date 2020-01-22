using Icon.Esb.Schemas.Wfm.Contracts;
using Icon.Framework;
using Icon.Logging;

namespace Icon.Services.ItemPublisher.Infrastructure.Models.Mappers
{
    public class UomMapper : IUomMapper
    {
        private ILogger<UomMapper> logger;

        public UomMapper(ILogger<UomMapper> logger)
        {
            this.logger = logger;
        }

        public WfmUomDescEnumType GetEsbUomDescription(string uomCode)
        {
            WfmUomDescEnumType esbUomDescription = WfmUomDescEnumType.EACH;
            uomCode = string.IsNullOrEmpty(uomCode) ? uomCode : uomCode.ToUpper();

            switch (uomCode)
            {
                case UomCodes.Each:
                    esbUomDescription = WfmUomDescEnumType.EACH;
                    break;
                case UomCodes.Pound:
                    esbUomDescription = WfmUomDescEnumType.POUND;
                    break;
                case UomCodes.Count:
                    esbUomDescription = WfmUomDescEnumType.COUNT;
                    break;
                case UomCodes.Ounce:
                    esbUomDescription = WfmUomDescEnumType.OUNCE;
                    break;
                case UomCodes.Case:
                    esbUomDescription = WfmUomDescEnumType.CASE;
                    break;
                case UomCodes.Pack:
                    esbUomDescription = WfmUomDescEnumType.PACK;
                    break;
                case UomCodes.Liter:
                    esbUomDescription = WfmUomDescEnumType.LITERS;
                    break;
                case UomCodes.Pint:
                    esbUomDescription = WfmUomDescEnumType.PINT;
                    break;
                case UomCodes.Kilogram:
                    esbUomDescription = WfmUomDescEnumType.KILOGRAM;
                    break;
                case UomCodes.Milliliter:
                    esbUomDescription = WfmUomDescEnumType.MILLILITERS;
                    break;
                case UomCodes.Gallon:
                    esbUomDescription = WfmUomDescEnumType.GALLON;
                    break;
                case UomCodes.Gram:
                    esbUomDescription = WfmUomDescEnumType.GRAMS;
                    break;
                case UomCodes.Centigram:
                    esbUomDescription = WfmUomDescEnumType.CENTIGRAMS;
                    break;
                case UomCodes.Feet:
                    esbUomDescription = WfmUomDescEnumType.FEET;
                    break;
                case UomCodes.Yard:
                    esbUomDescription = WfmUomDescEnumType.YARDS;
                    break;
                case UomCodes.Quart:
                    esbUomDescription = WfmUomDescEnumType.QUART;
                    break;
                case UomCodes.SquareFoot:
                    esbUomDescription = WfmUomDescEnumType.SQUAREFEET;
                    break;
                case UomCodes.Meter:
                    esbUomDescription = WfmUomDescEnumType.METERS;
                    break;
                case UomCodes.FluidOunces:
                    esbUomDescription = WfmUomDescEnumType.FLUIDOUNCES;
                    break;
                default:
                    logger.Warn(string.Format("No matching {0} for UOM {1}.  Defaulting to {2}.", typeof(WfmUomDescEnumType).Name, uomCode, esbUomDescription));
                    break;
            }

            return esbUomDescription;
        }

        public WfmUomCodeEnumType GetEsbUomCode(string uomCode)
        {
            WfmUomCodeEnumType esbUomCode = WfmUomCodeEnumType.EA;
            uomCode = string.IsNullOrEmpty(uomCode) ? uomCode : uomCode.ToUpper();

            switch (uomCode)
            {
                case UomCodes.Each:
                    esbUomCode = WfmUomCodeEnumType.EA;
                    break;
                case UomCodes.Pound:
                    esbUomCode = WfmUomCodeEnumType.LB;
                    break;
                case UomCodes.Count:
                    esbUomCode = WfmUomCodeEnumType.CT;
                    break;
                case UomCodes.Ounce:
                    esbUomCode = WfmUomCodeEnumType.OZ;
                    break;
                case UomCodes.Case:
                    esbUomCode = WfmUomCodeEnumType.CS;
                    break;
                case UomCodes.Pack:
                    esbUomCode = WfmUomCodeEnumType.PK;
                    break;
                case UomCodes.Liter:
                    esbUomCode = WfmUomCodeEnumType.LT;
                    break;
                case UomCodes.Pint:
                    esbUomCode = WfmUomCodeEnumType.PT;
                    break;
                case UomCodes.Kilogram:
                    esbUomCode = WfmUomCodeEnumType.KG;
                    break;
                case UomCodes.Milliliter:
                    esbUomCode = WfmUomCodeEnumType.ML;
                    break;
                case UomCodes.Gallon:
                    esbUomCode = WfmUomCodeEnumType.GL;
                    break;
                case UomCodes.Gram:
                    esbUomCode = WfmUomCodeEnumType.GR;
                    break;
                case UomCodes.Centigram:
                    esbUomCode = WfmUomCodeEnumType.CG;
                    break;
                case UomCodes.Feet:
                    esbUomCode = WfmUomCodeEnumType.FT;
                    break;
                case UomCodes.Yard:
                    esbUomCode = WfmUomCodeEnumType.YD;
                    break;
                case UomCodes.Quart:
                    esbUomCode = WfmUomCodeEnumType.QT;
                    break;
                case UomCodes.SquareFoot:
                    esbUomCode = WfmUomCodeEnumType.SF;
                    break;
                case UomCodes.Meter:
                    esbUomCode = WfmUomCodeEnumType.MT;
                    break;
                case UomCodes.FluidOunces:
                    esbUomCode = WfmUomCodeEnumType.FZ;
                    break;
                default:
                    logger.Warn(string.Format("No matching {0} for UOM {1}.  Defaulting to {2}.", typeof(WfmUomCodeEnumType).Name, uomCode, esbUomCode));
                    break;
            }

            return esbUomCode;
        }
    }
}
