import { useToast } from 'vue-toastification';

const toast = useToast();
const toastOptions = {
    timeout: 3000,
  };

export const handleApiErrorToast = (error) => {
  if (error.response && error.response.status !== 401) {
    const message = error.response.data.message;
    toast.error(message, toastOptions);
  } else {
    if (error.response.status !== 401) {
      toast.error('Сетевая ошибка', toastOptions);
      console.error('Ошибка API:', error);
    }
  }
  throw error;
};