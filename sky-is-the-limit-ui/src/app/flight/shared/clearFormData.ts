export function clearFormData() {
  sessionStorage.removeItem('bookFormData');
  sessionStorage.removeItem('personalInfoFormData');
  sessionStorage.removeItem('totalPrice');
}
