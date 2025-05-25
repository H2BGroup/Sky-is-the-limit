export function offerPurchasedMessage(offerId: string, data: any) {
  if (data && data.offerId === offerId) {
    return true;
  }
  return false;
}
