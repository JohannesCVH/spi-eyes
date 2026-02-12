const CACHE_NAME = 'v1';
const ASSETS = [
  '/',
  '/index.html',
  '/src/main.js'
];

// Install Event: Cache assets
self.addEventListener('install', (event) => {
  event.waitUntil(
    caches.open(CACHE_NAME).then((cache) => cache.addAll(ASSETS))
  );
});

// Fetch Event: Serve from cache if offline
self.addEventListener('fetch', (event) => {
  event.respondWith(
    caches.match(event.request).then((response) => {
      return response || fetch(event.request);
    })
  );
});

// self.addEventListener('push', (event) => {
//   let data;
//   if (event.data) {
//     data = event.data.json();
//   }
// })