import { defineStore } from 'pinia';
import { useCartStore } from '@/stores/cartStore';
import { loginUser, registerUser } from '@/api/user.js';
import router from '@/router';

export const useAuthStore = defineStore('auth', {
  state: () => ({
    token: localStorage.getItem('token') || null,
    role: localStorage.getItem('role') || null,
    userId: localStorage.getItem('userId') || null,
    name: localStorage.getItem('name') || null,
    isAdmin: JSON.parse(localStorage.getItem('isAdmin') || 'false'),
  }),

  actions: {
    async login(login, password) {
      try {
        const data = await loginUser(login, password);
        
        this.token = data.token;
        localStorage.setItem('token', this.token);
        
        // Декодируем payload из токена
        const payload = this.decodeToken(this.token);

        // Декодируем name из Base64, если оно есть (из-за возможной кириллицы)
        if (payload.name) {
          const decodedBytes = Uint8Array.from(atob(payload.name), c => c.charCodeAt(0));
          payload.name = new TextDecoder('utf-8').decode(decodedBytes);
        }

        if (!payload) throw new Error('Ошибка декодирования токена');

        this.saveUserData(payload);

        useCartStore().loadCart();

        return true;
      } catch (error) {
        return false;
        console.error('Ошибка авторизации:', error);
      }
    },

    async registration(newUser, password) {
      try {
        // Регистрация
        const result = await registerUser(newUser, password);

        // Авторизация сразу после регистрации
        if (result) {
          await this.login(newUser.login, password);
        }
      } catch (error) {
        throw error;
        console.error('Ошибка регистрации или авторизации:', error);
      }
    },

    logout() {
      try {
        useCartStore().clearCartOnLogout();
        this.clearUserData();
        router.push('/guest');
      } catch (error) {
        console.error('Ошибка при выходе:', error);
      }
    },

    decodeToken(token) {
      try {
        if (typeof token !== 'string' || token.split('.').length !== 3) {
          throw new Error('Неверный формат токена');
        }
    
        const payloadBase64 = token.split('.')[1].replace(/-/g, '+').replace(/_/g, '/');
        const payloadJson = atob(payloadBase64);
        return JSON.parse(payloadJson);
      } catch (error) {
        console.error('Ошибка декодирования токена:', error.message);
        return null;
      }
    },

    saveUserData(payload) {
      this.role = payload.role;
      this.userId = payload.guid;
      this.name = payload.name;
      this.isAdmin = ['Manager', 'Admin'].includes(this.role);

      localStorage.setItem('role', this.role);
      localStorage.setItem('userId', this.userId);
      localStorage.setItem('name', this.name);
      localStorage.setItem('isAdmin', JSON.stringify(this.isAdmin));
    },

    clearUserData() {
      this.token = this.role = this.userId = this.name = null;
      this.isAdmin = false;

      ['token', 'role', 'userId', 'name', 'isAdmin'].forEach((key) =>
        localStorage.removeItem(key)
      );
    },
  }
});
