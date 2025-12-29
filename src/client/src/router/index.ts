import { createRouter, createWebHistory } from 'vue-router'
import Dashboard from '../components/Dashboard.vue'
import Stream from '../components/Stream.vue'

const routes = [
  { path: '/', name: 'Dashboard', component: Dashboard },
  { path: '/stream/:cameraName', name: 'Stream', component: Stream, props: true }
];

const router = createRouter({
  // history: createWebHistory(import.meta.env.BASE_URL),
  history: createWebHistory(),
  routes
})

export default router
