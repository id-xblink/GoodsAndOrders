import { createRouter, createWebHistory } from 'vue-router';
import { useAuthStore } from '@/stores/authStore.js';

// Lazy-loading компонентов
const AppLayout = () => import('@/layouts/AppLayout.vue');
const AuthLayout = () => import('@/layouts/AuthLayout.vue');
const Login = () => import('@/views/LoginView.vue');
const Catalog = () => import('@/views/CatalogView.vue');
const CartView = () => import('@/views/CartView.vue');
const Orders = () => import('@/views/OrderView.vue');
const UserManagement = () => import('@/views/UserView.vue');
const GuestHome = () => import('@/views/HomeView.vue');
const Register = () => import('@/views/RegisterView.vue');
const NotFound = () => import('@/views/NotFoundView.vue');

const routes = [
  {
    path: '/',
    redirect: '/guest',
  },
  {
    path: '/guest',
    component: GuestHome,
  },
  {
    path: '/auth',
    component: AuthLayout,
    children: [
      { path: 'login', component: Login },
      { path: 'register', component: Register },
    ],
  },
  {
    path: '/app',
    component: AppLayout,
    children: [
      { path: 'users', component: UserManagement, meta: { requiresAuth: true } },
      { path: 'catalog', component: Catalog, meta: { requiresAuth: true } },
      { path: 'orders', component: Orders, meta: { requiresAuth: true } },
      { path: 'cart', component: CartView, meta: { requiresAuth: true } },
    ],
  },
  {
    path: '/:pathMatch(.*)*',
    component: NotFound,
  },
];

const router = createRouter({
  history: createWebHistory(),
  routes,
});

// Глобальная защита маршрутов
router.beforeEach((to, from, next) => {
  const authStore = useAuthStore();
  if (to.matched.some(record => record.meta.requiresAuth) && !authStore.token) {
    next('/auth/login'); // Перенаправление, если нет токена
  } else {
    next();
  }
});

export default router;