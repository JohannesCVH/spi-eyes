<template>
  <div class="site-container">
    <nav v-if="($route.name != 'Login' && $route.name != 'Register')" class="navbar mb-4">
      <div class="container-fluid">
        <router-link class="navbar-brand" to="/">
          <img src="./assets/logo.png" style="max-width: 48px; box-shadow: 0 4px 8px rgba(0,0,0,0.2); border-radius: 4px;"/>
        </router-link>

        <button @click="logout" class="btn btn-primary">
          Logout
        </button>
      </div>
    </nav>
    <div class="container">
      <router-view></router-view>
    </div>

    <toast ref="toast"></toast>
  </div>
</template>

<script setup lang="ts">
  import { ref, computed, inject, onMounted, provide } from 'vue';
  import { useRouter } from 'vue-router';
  import Toast from './components/Toast.vue';
  import Cookies from 'js-cookie'
  import { ToastType } from './models/ToastTypes';

  const config: any = inject('config');

  const router = useRouter();
  const siteBgColor = ref('#ece5d8');
  const toast = ref(null);

  onMounted(async () => {
    
  });

  provide('makeToast', (title, message, type: ToastType) => {
    if (toast.value) {
      toast.value.show(title, message, type);
    }
  });

  const logout = async () => {
    Cookies.remove('User.Id');
    router.push({ name: 'Login' });
  };
</script>

<style scoped>
  .site-container {
    background-color: v-bind(siteBgColor);
    height: 100vh;
  }
</style>