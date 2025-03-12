"use strict";

// Kết nối SignalR
const connection = new signalR.HubConnectionBuilder()
    .withUrl("/notificationHub")
    .withAutomaticReconnect()
    .build();

connection.start()
    .then(() => console.log('Connected to notification hub'))
    .catch(err => console.error('Error connecting to notification hub:', err));

// Lắng nghe sự kiện nhận thông báo
connection.on("ReceiveNotification", function (message, type) {
    showNotification(message, type);
});

function showNotification(message, type) {
    const container = document.getElementById("notification-container");
    const template = document.getElementById("notification-template");
    if (!container || !template) {
        console.error('Notification container or template not found');
        return;
    }

    const notification = template.content.cloneNode(true).querySelector(".notification");

    // Thêm style dựa trên loại thông báo
    const styles = {
        success: "border-l-4 border-green-500 bg-green-50",
        info: "border-l-4 border-blue-500 bg-blue-50",
        warning: "border-l-4 border-yellow-500 bg-yellow-50",
        danger: "border-l-4 border-red-500 bg-red-50"
    };

    notification.classList.add(styles[type] || styles.info);
    notification.querySelector(".notification-message").textContent = message;
    notification.querySelector(".notification-time").textContent = new Date().toLocaleTimeString();

    // Thêm icon
    const iconDiv = notification.querySelector(".notification-icon");
    iconDiv.innerHTML = getIconForType(type);

    // Xử lý nút đóng
    const closeBtn = notification.querySelector(".close-btn");
    closeBtn.onclick = () => {
        notification.classList.add("opacity-0");
        setTimeout(() => notification.remove(), 300);
    };

    // Thêm vào container
    container.appendChild(notification);

    // Tự động ẩn sau 5 giây
    setTimeout(() => {
        if (notification.parentElement) {
            notification.classList.add("opacity-0");
            setTimeout(() => notification.remove(), 300);
        }
    }, 5000);
}

function getIconForType(type) {
    const icons = {
        success: `<svg class="w-6 h-6 text-green-500" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                    <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M5 13l4 4L19 7" />
                 </svg>`,
        info: `<svg class="w-6 h-6 text-blue-500" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                    <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M13 16h-1v-4h-1m1-4h.01M21 12a9 9 0 11-18 0 9 9 0 0118 0z" />
               </svg>`,
        warning: `<svg class="w-6 h-6 text-yellow-500" fill="none" stroke="currentColor" viewBox="0 24 24">
                    <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 9v2m0 4h.01m-6.938 4h13.856c1.54 0 2.502-1.667 1.732-3L13.732 4c-.77-1.333-2.694-1.333-3.464 0L3.34 16c-.77 1.333.192 3 1.732 3z" />
                 </svg>`,
        danger: `<svg class="w-6 h-6 text-red-500" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                    <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 18L18 6M6 6l12 12" />
                </svg>`
    };
    return icons[type] || icons.info;
}
