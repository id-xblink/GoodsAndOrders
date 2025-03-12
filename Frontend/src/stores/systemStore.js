import { defineStore } from 'pinia';
import { ref, onMounted, onUnmounted } from 'vue';
import { checkServerStatus } from '@/api/health.js';


export const useSystemStore = defineStore('system', () => {
  const apiStatus = ref('Проверка подключения...');
  let intervalId = null;

  async function checkApiStatus() {
    try {
      await checkServerStatus();
      apiStatus.value = 'online';
    } catch {
      apiStatus.value = 'offline';
    }
  }

  onMounted(() => {
    checkApiStatus();
    intervalId = setInterval(checkApiStatus, 5000);
  })

  onUnmounted(() => {
    clearInterval(intervalId);
  })

  return { apiStatus };
});
