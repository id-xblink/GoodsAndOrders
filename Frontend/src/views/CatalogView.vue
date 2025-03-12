<template>
    <div class="container">
      <h1>
        <Package class="icon-title"/> Каталог товаров
      </h1>
  
      <div class="filter-card">
        <button v-if="authStore.isAdmin" @click="showAddModal = true" class="btn primary">
          Добавить товар
        </button>
  
        <input v-model="searchQuery" placeholder="Поиск по имени..." class="input" maxlength="20"/>
        <input v-model.number="minPrice" type="number" placeholder="Мин. цена" min="0" max="99999" @input="minPrice = validatePrice($event.target.value)" class="input" />
        <input v-model.number="maxPrice" type="number" placeholder="Макс. цена" min="0" max="99999" @input="maxPrice = validatePrice($event.target.value)" class="input" />
        
        <select v-model="selectedCategory" class="select">
          <option value="">Все категории</option>
          <option v-for="category in categories" :key="category.id" :value="category.id">
            {{ category.name }}
          </option>
        </select>
        <button @click="applyFilter" class="btn filter">Применить фильтры</button>
      </div>
  
      <div v-if="isLoading" class="spinner"></div>
      <DataTable v-else :columns="columns" :data="filteredProducts" :role="authStore.role" @addToCart="openCartModal" @edit="openEditModal" @delete="deleteProduct"/>
  
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
    <!-- Модальное окно для добавления товара -->
    <transition name="modal">
      <div v-if="showAddModal" class="modal">
        <div class="modal-content">
          <h3>Добавить товар</h3>
          
          <input v-model="newProduct.name" placeholder="Название" class="input" maxlength="20"/>
          <input v-model.number="newProduct.price" type="number" min="0" max="999999" @input="newProduct.price = validatePrice($event.target.value)"  placeholder="Цена" class="input" />
  
          <select v-model="newProduct.categoryId" class="select">
            <option v-for="category in categories" :key="category.id" :value="category.id">
              {{ category.name }}
            </option>
          </select>
          <div class="modal-actions">
            <button @click="addProduct" class="btn primary">Сохранить</button>
            <button @click="closeAddModal" class="btn danger">Отмена</button>
          </div>
        </div>
      </div>
    </transition>
    <!-- Модальное окно для редактирования товара -->
    <transition name="modal">
      <div v-if="showEditModal" class="modal">
        <div class="modal-content">
          <h3>Редактировать товар</h3>
          
          <input v-model="editProduct.name" placeholder="Название" class="input" maxlength="20"/>
          <input v-model.number="editProduct.price" type="number" min="0" max="99999" @input="editProduct.price = validatePrice($event.target.value)" placeholder="Цена" class="input" />
  
          <select v-model="editProduct.categoryId" class="select">
            <option v-for="category in categories" :key="category.id" :value="category.id">
              {{ category.name }}
            </option>
          </select>
          <div class="modal-actions">
            <button @click="updateProduct" class="btn primary">Сохранить</button>
            <button @click="closeShowModal" class="btn danger">Отмена</button>
          </div>
        </div>
      </div>
    </transition>
    <transition name="modal">
      <div v-if="showCartModal" class="modal">
        <div class="modal-content">
          <h3>Выберите количество</h3>
          <input type="number" v-model="selectedQuantity" min="1" max="1000" @input="selectedQuantity = validateCount($event.target.value)" class="input" step="1" @keydown="blockDotComma"/>
          <div class="modal-actions">
            <button @click="addToCart" class="btn primary">Добавить</button>
            <button @click="showCartModal = false" class="btn danger">Отменить</button>
          </div>
        </div>
      </div>
    </transition>
  </template>
  
  <script setup>
  import { ref, computed, onMounted } from 'vue';
  import DataTable from '@/components/DataTableProductComponent.vue';
  
  
  import { useAuthStore } from '@/stores/authStore';
  import { useCartStore } from '@/stores/cartStore';
  import { useCatalogStore } from '@/stores/catalogStore';
  
  import { useToast } from 'vue-toastification';
  import Swal from "sweetalert2";
  import { ArrowLeft, ArrowRight, Package } from 'lucide-vue-next';
  
  const toast = useToast();
  const toastOptions = {
    timeout: 3000,
  };
  
  const blockDotComma = (event) => {
      if (event.key === '.' || event.key === ',') {
        event.preventDefault();
      }
    }
  
  const cartStore = useCartStore();
  const authStore = useAuthStore();
  const catalogStore = useCatalogStore();
  const columns = ['name', 'code', 'price', 'categoryName', 'actions'];
  const categories = ref([]);
  const searchQuery = ref('');
  const minPrice = ref(null);
  const maxPrice = ref(null);
  const showAddModal = ref(false);
  const showEditModal = ref(false);
  const showCartModal = ref(false);
  const editProduct = ref({});
  const selectedProduct = ref({});
  const selectedQuantity = ref({});
  const newProduct = ref({ name: '', price: '' });
  const isLoading = ref(true);
  const selectedCategory = ref("");
  
  const currentPage = ref(1);
  const pageSize = ref(5);
  const totalPages = ref(1);
  const totalItems = ref(0);
  
  const filteredProducts = ref({});
  
  let params = {
    page: currentPage.value,
    pageSize: pageSize.value,
    search: searchQuery.value,
    minPrice: minPrice.value,
    maxPrice: maxPrice.value,
    categoryId: selectedCategory.value || null,
  };
  
  // Универсальная функция валидации чисел
  const validatePrice = (value) => {
    if (value === '') return null;
    let num = Number(value);
    return isNaN(num) ? 0 : Math.min(Math.max(num, 0), 999999);
  };
  
  const validateCount = (value) => {
    if (value === '') return null;
    let num = Number(value);
    return isNaN(num) ? 0 : Math.min(Math.max(num, 0), 1000);
  };
  
  const prevPage = () => {
    if (currentPage.value > 1) {
      currentPage.value--;
      params.page = currentPage.value;
      fetchProducts();
    }
  };
  
  const nextPage = () => {
    if (currentPage.value < totalPages.value) {
      currentPage.value++;
      params.page = currentPage.value;
      fetchProducts();
    }
  };
  
  const closeAddModal = () => {
    newProduct.value = {};
    showAddModal.value = false;
  };
  
  const closeShowModal = () => {
    editProduct.value = {};
    showEditModal.value = false;
  };
  
  const applyFilter = async () => {
    currentPage.value = 1;
    params = {
      page: currentPage.value,
      pageSize: pageSize.value,
      search: searchQuery.value,
      minPrice: minPrice.value,
      maxPrice: maxPrice.value,
      categoryId: selectedCategory.value ? selectedCategory.value : null,
    };
    fetchProducts();
  };
  
  const refreshData = async () => {
    currentPage.value = 1;
    fetchProducts();
  }
  
  const openEditModal = (product) => {
    editProduct.value = { ...product };
    
    const foundCategory = categories.value.find(cat => cat.name === product.categoryName);
    editProduct.value.categoryId = foundCategory ? foundCategory.id : null;
  
    showEditModal.value = true;
  };
  
  const openCartModal = (product) => {
    selectedProduct.value = { ...product };
    selectedQuantity.value = 1;
    showCartModal.value = true;
  };
  
  const fetchProducts = async () => {
    isLoading.value = true;
    try {
      const data = await catalogStore.getProducts(params);
  
      filteredProducts.value = data.items;
      totalPages.value = data.totalPages;
      totalItems.value = data.totalItems;
    } catch {
      
    } finally {
      isLoading.value = false;
    }
  };
  
  const fetchCategories = async () => {
    try {
      categories.value = await catalogStore.getCategories();
    } catch {
      
    }
  };
  
  const addToCart = async () => {
    if (!selectedQuantity.value) {
      toast.error("Некорректный ввод количества", toastOptions);
      return
    }
  
    try {
      cartStore.addToCart(selectedProduct.value, selectedQuantity.value)
      showCartModal.value = false;
      toast.info('Товар добавлен в корзину!', toastOptions);
    } catch (error) {
      toast.error('Нельзя добавить больше 1000 единиц товара', toastOptions);
      
    }
  };
  
  const addProduct = async () => {
    const error = validateProduct(newProduct.value, false);
    if (error) {
      toast.warning(error, toastOptions);
      return;
    }
  
    try {
      await catalogStore.addProduct(newProduct.value);
      toast.info("Товар добавлен!", toastOptions);
      refreshData();
      closeAddModal();
    } catch {
      
    }
  };
  
  const updateProduct = async () => {
    const error = validateProduct(editProduct.value, false);
    if (error) {
      toast.warning(error, toastOptions);
      return;
    }
  
    try {
      await catalogStore.updateProduct(editProduct.value.id, editProduct.value);
      toast.info("Товар изменён!", toastOptions);
      refreshData();
      closeShowModal();
    } catch {
      
    }
  };
  
  const deleteProduct = async (id) => {
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
      await catalogStore.deleteProduct(id);
      toast.info('Товар удалён', toastOptions);
      refreshData();
    } catch {
      
    }
  };
  
  const validateProduct = (product, isNew = false) => {
    if (!product.name || !product.price || !product.categoryId)
      return "Не все поля заполнены";
  
    if (!product.name.trim())
      return "Имя не может состоять из пробелов";
  
    if (product.price <= 0)
      return "Цена не может быть меньше или равна 0";
  
    return null;
  };
  
  onMounted(() => {
    fetchProducts();
    fetchCategories();
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
  