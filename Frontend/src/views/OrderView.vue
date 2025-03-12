<template>
    <div>
      <h1 v-if="authStore.isAdmin">
        <ScrollText class="icon-title"/> Заказы
      </h1>
      <h1 v-else>
        <ScrollText class="icon-title"/> Мои заказы
      </h1>
  
      <div class="filter-card">
        <select v-model="selectedStatus" class="select">
          <option value="">Все статусы</option>
          <option v-for="status in statuses" :key="status" :value="status.id">{{ status.name }}</option>
        </select>
        <button @click="applyFilter" class="btn filter">Применить фильтры</button>
      </div>
      <div v-if="isLoading" class="spinner"></div>
      <DataTable v-else :columns="columns" :data="filteredOrders" :role="authStore.isAdmin" @begin="openDateModal" @check="openOrderDetails" @finish="updateOrderStatus" @delete="deleteOrder"/>
  
      <div class="pagination">
        <button @click="prevPage" :disabled="currentPage <= 1" class="btn pagination-btn">
          <ArrowLeft class="icon" /> Назад
        </button>
        <span class="pagination-span">Страница {{ currentPage }} из {{ totalPages || 1 }}</span>
        <button @click="nextPage" :disabled="currentPage >= totalPages" class="btn pagination-btn">
          Вперёд <ArrowRight class="icon" />
        </button>
      </div>
  
      <div v-if="showModal" class="modal">
        <div class="modal-content">
          <h3>Товары в заказе #{{ selectedOrder?.orderNumber }}</h3>
          <div class="table-container">
            <table>
              <thead>
                <tr>
                  <th>Название</th>
                  <th>Цена</th>
                  <th>Кол-во</th>
                  <th>Сумма</th>
                </tr>
              </thead>
              <tbody>
                <tr v-for="item in orderItems" :key="item.productId">
                  <td>{{ item.productName }}</td>
                  <td>{{ item.price }} ₽</td>
                  <td>{{ item.quantity }}</td>
                  <td>{{ item.price * item.quantity }} ₽</td>
                </tr>
              </tbody>
            </table>
          </div>
          <p><strong>Итог:</strong> {{ totalPrice }} ₽</p>
          <button class="modal-button close-btn" @click="showModal = false">Закрыть</button>
        </div>
      </div>
      <!-- Модальное окно выбора даты -->
      <div v-if="showDateModal" class="modal">
        <div class="modal-content">
          <h3>Выберите дату выполнения</h3>
          <input type="date" v-model="selectedDate" :min="minDate" :max="maxDate">
          <div class="modal-actions">
            <button class="modal-button" @click="confirmStartOrder">Подтвердить</button>
            <button class="modal-button close-btn" @click="showDateModal = false">Закрыть</button>
          </div>
        </div>
      </div>
    </div>
  </template>
  
  <script setup>
  import { ref, computed, onMounted } from 'vue';
  import DataTable from '@/components/DataTableOrderComponent.vue';
  import { useOrderStore } from '@/stores/orderStore';
  import { useAuthStore } from '@/stores/authStore';
  import { useToast } from 'vue-toastification';
  import Swal from "sweetalert2";
  import { ArrowLeft, ArrowRight, ScrollText } from 'lucide-vue-next';
  
  const toast = useToast();
  const toastOptions = {
    timeout: 3000,
  };
  
  const authStore = useAuthStore();
  const orderStore = useOrderStore();
  const selectedStatus = ref('');
  const statuses = ref([]);
  
  const showModal = ref(false);
  const selectedOrder = ref(null);
  const orderItems = ref([]);
  const columns = ['orderNumber', 'orderDate', 'status', 'shipmentDate', 'actions'];
  const isLoading = ref(true);
  const showDateModal = ref(false);
  const selectedDate = ref('');
  const filteredOrders = ref([]);
  
  
  const currentPage = ref(1);
  const pageSize = ref(5);
  const totalPages = ref(1);
  const totalItems = ref(0);
  
  let params = {
    page: currentPage.value,
    pageSize: pageSize.value,
    orderStatusId: selectedStatus.value ? selectedStatus.value : null,
  };
  
  const prevPage = () => {
    if (currentPage.value > 1) {
      currentPage.value--;
      params.page = currentPage.value;
      fetchOrders();
    }
  };
  
  const nextPage = () => {
    if (currentPage.value < totalPages.value) {
      currentPage.value++;
      params.page = currentPage.value;
      fetchOrders();
    }
  };
  
  const openOrderDetails = async (order) => {
    selectedOrder.value = order;
    showModal.value = true;
  
    try {
      const data = await orderStore.getDetailedOrder(order.id);
      orderItems.value = data.items;
    } catch {
      
    }
  };
  
  const applyFilter = async () => {
    currentPage.value = 1;
    params = {
      page: currentPage.value,
      pageSize: pageSize.value,
      orderStatusId: selectedStatus.value ? selectedStatus.value : null,
    };
    fetchOrders();
  };
  
  const refreshData = async () => {
    currentPage.value = 1;
    fetchOrders();
  };
  
  const today = new Date();
  const minDate = today.toISOString().split("T")[0];
  
  const maxDate = (() => {
    const date = today;
    date.setDate(date.getDate() + 14);
    return date.toISOString().split("T")[0];
  })();
  
  // Итоговая сумма заказа
  const totalPrice = computed(() => 
    orderItems.value.reduce((sum, item) => sum + item.price * item.quantity, 0)
  );
  
  const openDateModal = async (order) => {
        selectedOrder.value = order;
        showDateModal.value = true;
        selectedDate.value = minDate;
  };
  
  const closeDateModal = async () => {
    selectedOrder.value = null;
    showDateModal.value = false;
    selectedDate.value = null;
  };
  
  const fetchStatuses = async () => {
    try {
      const data = await orderStore.getStatuses();
      statuses.value = data;
    } catch {
      
    }
  };
  const fetchOrders = async () => {
    try {
      const data = await orderStore.getUserOrders(authStore.isAdmin, authStore.userId, params);
      filteredOrders.value = data.orders;
      totalPages.value = data.totalPages;
      totalItems.value = data.totalItems;
    } catch {
      
    } finally {
      isLoading.value = false;
    }
  };
  
  const confirmStartOrder = async () => {
    try {
      if (!selectedDate.value) {
        toast.warning("Введите дату!", toastOptions);
        return;
      }
    const orderUpdate = {
      id: selectedOrder.value.id,
      shipmentDate: new Date(selectedDate.value).toISOString(),
    };
  
      await orderStore.updateOrderToStart(orderUpdate);
      toast.info("Статус заказа изменён!", toastOptions);
      closeDateModal();
      refreshData();
    }
    catch {
      
    }
  };
  
  const updateOrderStatus = async (order) => {
    const result = await Swal.fire({
      title: "Вы уверены?",
      text: "Заказ будет закрыт!",
      icon: "info",
      showCancelButton: true,
      confirmButtonColor: "green",
      cancelButtonColor: "#3085d6",
      confirmButtonText: "Закрыть заказ",
      cancelButtonText: "Отмена",
    });
  
    if (!result.isConfirmed) return;
  
    try {
      const orderUpdate = {
        id: order.id,
        shipmentDate: null,
      };
  
      await orderStore.updateOrderToFinish(orderUpdate);
      toast.success("Заказ закрыт", toastOptions);
      refreshData();
    } catch {
      
    }
  };
  
  const deleteOrder = async (orderId) => {
    const result = await Swal.fire({
      title: "Вы уверены?",
      text: "Это действие нельзя отменить!",
      icon: "warning",
      showCancelButton: true,
      confirmButtonColor: "#d33",
      cancelButtonColor: "#3085d6",
      confirmButtonText: "Удалить",
      cancelButtonText: "Отмена",
    });
  
    if (!result.isConfirmed) return;
  
    try {
      await orderStore.deleteOrder(orderId);
      toast.info("Заказ успешно удалён!", toastOptions);
      refreshData();
    } catch {
      
    }
  };
  
  onMounted(() => {
    fetchOrders();
    fetchStatuses();
  });
  </script>
  
  <style scoped>
    .filter-card {
      background: #f9f9f9;
      padding: 15px;
      border-radius: 8px;
      display: flex;
      gap: 10px;
      align-items: center;
      margin-bottom: 20px;
    }
  
    .input, .select {
      padding: 8px;
      border: 1px solid #ddd;
      border-radius: 4px;
      width: 100%;
    }
  
    .btn {
      padding: 8px 12px;
      border: none;
      border-radius: 4px;
      cursor: pointer;
      transition: 0.2s;
    }
  
    .filter {
      background-color: #2eb0e4;
    }
  
    .btn.primary {
      background-color: #007bff;
      color: white;
    }
  
    .btn.primary:hover {
      background-color: #0056b3;
    }
  
    .btn.danger {
      background-color: #dc3545;
      color: white;
    }
  
    .btn.danger:hover {
      background-color: #c82333;
    }
  
    .pagination {
      display: flex;
      justify-content: center;
      align-items: center;
      margin-top: 20px;
    }
  
    .pagination-span {
      margin: 20px;
    }
  
    .modal-actions {
      display: flex;
      gap: 10px;
    }
  
    .modal-actions button {
      flex: 1;
    }
  
    .spinner {
      width: 50px;
      height: 50px;
      border: 5px solid #ddd;
      border-top-color: #007bff;
      border-radius: 50%;
      animation: spin 1s ease-in-out infinite;
      margin: 20px auto;
    }
  
    h1, h3 {
      text-align: center;
      
    }
  
    * {
      box-sizing: border-box;
    }
  
    @keyframes spin {
      0% { transform: rotate(0deg); }
      100% { transform: rotate(360deg); }
    }
  
  .modal {
    position: fixed;
    top: 0;
    left: 0;
    width: 100%;
    height: 100%;
    background: rgba(0, 0, 0, 0.4); 
    display: flex;
    align-items: center;
    justify-content: center;
    z-index: 1000;
    backdrop-filter: blur(5px); 
  }
  
  .modal-content {
    background: white;
    padding: 20px;
    border-radius: 12px;
    box-shadow: 0 4px 10px rgba(0, 0, 0, 0.2);
    width: 500px;
    max-width: 90%;
    text-align: center;
    animation: fadeIn 0.3s ease-in-out;
    display: flex;
    flex-direction: column;
    gap: 10px;
  }
  
  .modal-button {
    background: #007bff;
    color: white;
    padding: 10px 15px;
    border: none;
    border-radius: 6px;
    cursor: pointer;
    font-size: 16px;
    transition: background 0.2s ease-in-out;
  }
  
  .modal-button:hover {
    background: #0056b3;
  }
  
  .close-btn {
    background: rgb(255, 43, 43);
    top: 10px;
    right: 10px;
    border: none;
    font-size: 18px;
    cursor: pointer;
    color: #000000;
    transition: color 0.2s;
  }
  
  .close-btn:hover {
    background: #920505;
    color: white;
  }
  
  h3 {
    font-size: 20px;
    margin-bottom: 15px;
  }

  .table-container {
    max-height: 300px;
    overflow-y: auto;
    overflow-x: auto;
    border: 1px solid #ccc;
  }
  
  table {
    width: 100%;
    border-collapse: collapse;
    margin-top: 10px;
  }
  
  th, td {
    padding: 10px;
    border-bottom: 1px solid #ddd;
    text-align: left;
  }
  
  th {
    background: #f4f4f4;
  }
  
  p {
    font-size: 18px;
    margin-top: 15px;
  }
  
  
  
  input[type="date"] {
    padding: 8px;
    border-radius: 6px;
    border: 1px solid #ccc;
    font-size: 16px;
  }
  
  @keyframes fadeIn {
    from { opacity: 0; transform: scale(0.9); }
    to { opacity: 1; transform: scale(1); }
  }
  
  .icon {
    width: 18px;
    height: 18px;
  }
  
  .pagination-btn {
    display: flex;
    align-items: center;
    gap: 8px;
    padding: 8px 16px;
    border: none;
    border-radius: 8px;
    font-size: 16px;
    font-weight: bold;
    cursor: pointer;
    transition: all 0.3s ease;
    color: white;
    background: #007bff;
  }
  
  .pagination-btn:hover {
    transform: scale(1.05);
  }
  
  .pagination-btn:disabled {
    background: #ccc;
    cursor: not-allowed;
    transform: none;
  }
  
  h1 {
    display: flex;
    align-items: center;
    justify-content: center;
    gap: 10px; 
    text-align: center;
    font-size: 24px;
  }
  
  .icon-title {
    width: 50px;
    height: 50px;
  }
  </style>