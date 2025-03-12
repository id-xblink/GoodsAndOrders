<template>
    <div class="container">
      <h1>
        <Users class="icon-title"/> Список пользователей
      </h1>
      
      <div class="filter-card">
        <button v-if="isAdmin" @click="showAddModal = true" class="btn primary">
          Добавить пользователя
        </button>
        <input v-model="searchQuery" placeholder="Поиск по коду..." maxlength="9" class="input" />
        
        <select v-model="selectedRole" class="select">
          <option value="">Все роли</option>
          <option v-for="role in roles" :key="role.id" :value="role.id">{{ role.name }}</option>
        </select>
        <button @click="applyFilter" class="btn filter">Применить фильтры</button>
      </div>
      
      <div v-if="isLoading" class="spinner"></div>
      <DataTable v-else :columns="columns" :data="filteredUsers" :role="authStore.role" @edit="openEditModal" @delete="deleteUser"/>
      
      <div class="pagination">
        <button @click="prevPage" :disabled="currentPage <= 1" class="btn pagination-btn">
          <ArrowLeft class="icon" /> Назад
        </button>
        <span class="pagination-span">Страница {{ currentPage }} из {{ totalPages || 1 }}</span>
        <button @click="nextPage" :disabled="currentPage >= totalPages" class="btn pagination-btn">
          Вперёд <ArrowRight class="icon" />
        </button>
      </div>
    </div>
    
    <!-- Модальное окно для добавления пользователя -->
    <transition name="modal">
      <div v-if="showAddModal" class="modal">
        <div class="modal-content">
          <h3>Добавить пользователя</h3>
          <input v-model="newUser.name" placeholder="Имя" class="input" maxlength="30"/>
          <input v-model="newUser.login" placeholder="Логин" class="input" maxlength="20"/>
          <input v-model="userPassword" type="password" placeholder="Пароль" class="input" maxlength="20"/>
          <input v-model="newUser.address" placeholder="Адрес" class="input" maxlength="200"/>
          <input v-model="newUser.discount" type="number" min="0" max="100" placeholder="Скидка" step="1" @keydown="blockDotComma" class="input"/>
          <select v-model="newUser.userRoleId" class="select">
            <option v-for="role in roles" :key="role.id" :value="role.id">{{ role.name }}</option>
          </select>
          <div class="modal-actions">
            <button @click="addUser" class="btn primary">Сохранить</button>
            <button @click="closeAddModal" class="btn danger">Отмена</button>
          </div>
        </div>
      </div>
    </transition>
    <!-- Модальное окно для редактирования пользователя -->
    <transition name="modal">
      <div v-if="showEditModal" class="modal">
        <div class="modal-content">
          <h3>Редактировать пользователя</h3>
          
          <input v-model="editUser.name" placeholder="Имя" class="input" maxlength="30"/>
          <input v-model="editUser.address" placeholder="Адрес" class="input" maxlength="200"/>
          <input v-model.number="editUser.discount" type="number" min="0" max="0" placeholder="Скидка" class="input" />
  
          <div class="modal-actions">
            <button @click="updateUser" class="btn primary">Сохранить</button>
            <button @click="closeEditModal" class="btn danger">Отмена</button>
          </div>
        </div>
      </div>
    </transition>
  </template>
  
  <script setup>
    import { ref, computed, onMounted } from 'vue';
    import DataTable from '@/components/DataTableUserComponent.vue';
    import { useUserStore } from '@/stores/userStore';
    import { useAuthStore } from '@/stores/authStore';
    import { useToast } from 'vue-toastification';
    import Swal from "sweetalert2";
    import { ArrowLeft, ArrowRight, Users } from 'lucide-vue-next';
  
    const toast = useToast();
    const toastOptions = {
      timeout: 3000,
    };
  
    const authStore = useAuthStore();
    const userStore = useUserStore();
    const columns = ['name', 'login', 'code', 'address', 'discount', 'userRole', 'actions'];
    const roles = ref([]);
    const searchQuery = ref('');
    const selectedRole = ref('');
    const userPassword = ref('');
    const showAddModal = ref(false);
    const showEditModal = ref(false);
    const editUser = ref({});
    const newUser = ref({});
    const isLoading = ref(true);
  
    const filteredUsers = ref([]);
  
    const currentPage = ref(1);
    const pageSize = ref(5);
    const totalPages = ref(1);
    const totalItems = ref(0);
  
    const blockDotComma = (event) => {
      if (event.key === '.' || event.key === ',') {
        event.preventDefault();
      }
    }
    const isAdmin = computed(() => authStore.isAdmin);
  
    let params = {
      page: currentPage.value,
      pageSize: pageSize.value,
      search: searchQuery.value,
      userRoleId: selectedRole.value ? selectedRole.value : null,
    };
  
    const prevPage = () => {
      if (currentPage.value > 1) {
        currentPage.value--;
        params.page = currentPage.value;
        fetchUsers();
      }
    };
  
    const nextPage = () => {
      if (currentPage.value < totalPages.value) {
        currentPage.value++;
        params.page = currentPage.value;
        fetchUsers();
      }
    };
  
    const applyFilter = async () => {
      currentPage.value = 1;
      params = {
        page: currentPage.value,
        pageSize: pageSize.value,
        search: searchQuery.value,
        userRoleId: selectedRole.value ? selectedRole.value : null,
      };
      fetchUsers();
    };
  
    const refreshData = async () => {
      currentPage.value = 1;
      fetchUsers();
    };
  
    const openEditModal = (user) => {
      editUser.value = { ...user };
      showEditModal.value = true;
    };
  
    const closeAddModal = () => {
      showAddModal.value = false;
      newUser.value = {};
      userPassword.value = '';
    };
  
    const closeEditModal = () => {
      showEditModal.value = false;
      editUser.value = {};
    };
  
    const fetchUsers = async () => {
      isLoading.value = true;
      try {
        const data = await userStore.getUsers(params);
        filteredUsers.value = data.users;
        totalPages.value = data.totalPages;
        totalItems.value = data.totalItems;
      } catch {
        
      } finally {
        isLoading.value = false;
      }
    };
  
    const fetchRoles = async () => {
      try { 
        roles.value = await userStore.getRoles();
      } catch {
        
      }
    };
  
    const addUser = async () => {
      const error = validateUser(newUser.value, true);
      if (error) {
        toast.warning(error, toastOptions);
        return;
      }
  
      try {
        await userStore.addUser(newUser.value, userPassword.value);
        toast.info("Пользователь добавлен!", toastOptions);
        refreshData();
        closeAddModal();
      } catch {
        
      }
    };
  
    const updateUser = async () => {
      const error = validateUser(editUser.value, false);
      if (error) {
        toast.warning(error, toastOptions);
        return;
      }
  
      try {
        await userStore.updateUser(editUser.value.id, editUser.value);
        toast.info("Пользователь изменён!", toastOptions);
        refreshData();
        closeEditModal();
      } catch {
        
      }
    };
  
    const deleteUser = async (id) => {
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
        await userStore.deleteUser(id);
        toast.info("Пользователь удалён!", toastOptions);
        refreshData();
      } catch {
  
      }
    };
  
    const validateUser = (user, isNew = false) => {
      if (!user.name || !user.address || !Number.isFinite(user.discount))
        return "Не все поля заполнены";
  
      if (!user.name.trim() || !user.address.trim())
        return "Поля не могут состоять из пробелов";
  
      if (user.discount < 0 || user.discount > 100)
        return "Скидка должна быть от 0 до 100";
  
      if (isNew) {
        if (!user.login || !user.userRoleId || !userPassword.value)
          return "Не все поля заполнены";
        if (!user.login.trim() || !userPassword.value.trim())
          return "Поля не могут состоять из пробелов";
        if (userPassword.value.length < 5)
          return "Пароль должен содержать минимум 5 символов!";
      }
      return null;
    };
  
    onMounted(() => {
        fetchUsers();
        fetchRoles();
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
  
    .modal {
      position: fixed;
      top: 0;
      left: 0;
      width: 100%;
      height: 100%;
      background: rgba(0, 0, 0, 0.5);
      display: flex;
      justify-content: center;
      align-items: center;
    }
  
    .modal-content {
      background: white;
      padding: 20px;
      border-radius: 8px;
      width: 500px;
      display: flex;
      flex-direction: column;
      gap: 10px;
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