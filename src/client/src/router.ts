import { createRouter, createWebHistory } from 'vue-router'
import Cookies from 'js-cookie'

import Dashboard from './components/Dashboard.vue'
import Stream from './components/Stream.vue'
import Login from './components/Login.vue';
import Register from './components/Register.vue';

const routes = [
  { path: '/', name: 'Dashboard', component: Dashboard },
  { path: '/stream/:cameraName', name: 'Stream', component: Stream, props: true },
  { path: '/login', name: 'Login', component: Login, props: false },
  { path: '/register', name: 'Register', component: Register, props: false }
];

const router = createRouter({
  history: createWebHistory(),
  routes
});

router.beforeEach((to, from, next) => {
  const userId = Cookies.get('User.Id');
  if (!userId && to.name != 'Login' && to.name != 'Register') {
    next({ name: 'Login' });
  } else {
    next()
  }
});

export default router