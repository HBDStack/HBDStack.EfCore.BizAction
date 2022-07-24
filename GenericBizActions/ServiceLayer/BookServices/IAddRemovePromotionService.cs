using DataLayer.EfClasses;
using HBDStack.StatusGeneric;

namespace ServiceLayer.BookServices;

public interface IAddRemovePromotionService
{
    IStatusGeneric Status { get; }

    AddRemovePromotionDto GetOriginal(int id);

    Book AddPromotion(AddRemovePromotionDto dto);
    Book RemovePromotion(int id);
}