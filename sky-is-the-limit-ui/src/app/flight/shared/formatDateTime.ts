import dayjs from 'dayjs';
import 'dayjs/locale/pl';

export function formatDateTime(datetime: string): string {
  return dayjs(datetime).locale('pl').format('DD.MM.YYYY HH:mm');
}
