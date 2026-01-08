# Spi-Eyes

NVR program written in C# and Vue.js.

At the moment it reads RTSP streams with FFmpeg and encodes them to HLS. There are separate FFmpeg processes to read individual frames and output them as images to a folder. These frame images are used for thumbnails on the dashboard and will later be used for object detection.

The Vue.js client currently has a simple dashboard that allows navigating to the different camera streams.