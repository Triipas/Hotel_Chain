class ChatBot {
    constructor() {
        this.isOpen = false;
        this.messages = [];
        this.init();
    }

    init() {
        this.createChatWidget();
        this.attachEventListeners();
        this.addWelcomeMessage();
    }

    createChatWidget() {
        const widget = document.createElement('div');
        widget.className = 'chat-widget';
        widget.innerHTML = `
            <!-- Botón flotante -->
            <button class="chat-toggle-btn" id="chatToggleBtn" aria-label="Abrir chat">
                💬
            </button>

            <!-- Ventana del chat -->
            <div class="chat-window" id="chatWindow">
                <div class="chat-header">
                    <h3>🤖 Asistente Virtual</h3>
                    <button class="chat-close" id="chatCloseBtn" aria-label="Cerrar chat">×</button>
                </div>

                <div class="chat-body">
                    <div class="chat-messages" id="chatMessages">
                        <!-- Mensajes aquí -->
                    </div>
                </div>

                <div class="chat-footer">
                    <div class="chat-input-container">
                        <input 
                            type="text" 
                            class="chat-input" 
                            id="chatInput" 
                            placeholder="Escribe tu mensaje..."
                            autocomplete="off"
                        />
                        <button class="chat-send-btn" id="chatSendBtn" aria-label="Enviar mensaje">
                            ➤
                        </button>
                    </div>
                </div>
            </div>
        `;
        document.body.appendChild(widget);
    }

    attachEventListeners() {
        const toggleBtn = document.getElementById('chatToggleBtn');
        const closeBtn = document.getElementById('chatCloseBtn');
        const sendBtn = document.getElementById('chatSendBtn');
        const input = document.getElementById('chatInput');

        toggleBtn.addEventListener('click', () => this.toggleChat());
        closeBtn.addEventListener('click', () => this.closeChat());
        sendBtn.addEventListener('click', () => this.sendMessage());
        
        input.addEventListener('keypress', (e) => {
            if (e.key === 'Enter') {
                this.sendMessage();
            }
        });
    }

    toggleChat() {
        const chatWindow = document.getElementById('chatWindow');
        this.isOpen = !this.isOpen;
        
        if (this.isOpen) {
            chatWindow.classList.add('active');
            document.getElementById('chatInput').focus();
        } else {
            chatWindow.classList.remove('active');
        }
    }

    closeChat() {
        const chatWindow = document.getElementById('chatWindow');
        chatWindow.classList.remove('active');
        this.isOpen = false;
    }

    addWelcomeMessage() {
        const messagesContainer = document.getElementById('chatMessages');
        messagesContainer.innerHTML = `
            <div class="welcome-message">
                <h4>¡Hola! 👋</h4>
                <p>Soy tu asistente virtual de Costa Dorada. ¿En qué puedo ayudarte hoy?</p>
            </div>
        `;
    }

    async sendMessage() {
        const input = document.getElementById('chatInput');
        const message = input.value.trim();

        if (!message) return;

        this.addMessage(message, 'user');
        input.value = '';

        this.showTypingIndicator();

        try {
            const response = await fetch('/api/chat/message', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({ message })
            });

            const data = await response.json();

            this.hideTypingIndicator();

            if (response.ok) {
                // Agregar respuesta del bot
                this.addMessage(data.response, 'bot');
            } else {
                this.addMessage('Lo siento, hubo un error. Por favor intenta de nuevo.', 'bot');
            }
        } catch (error) {
            this.hideTypingIndicator();
            this.addMessage('Error de conexión. Verifica tu internet.', 'bot');
            console.error('Error:', error);
        }
    }

    addMessage(text, type) {
        const messagesContainer = document.getElementById('chatMessages');
        const messageDiv = document.createElement('div');
        messageDiv.className = `chat-message ${type}`;

        const avatar = type === 'bot' ? '🤖' : '👤';
        
        messageDiv.innerHTML = `
            <div class="message-avatar ${type}">${avatar}</div>
            <div class="message-content ${type}">${this.escapeHtml(text)}</div>
        `;

        messagesContainer.appendChild(messageDiv);
        messagesContainer.scrollTop = messagesContainer.scrollHeight;
    }

    showTypingIndicator() {
        const messagesContainer = document.getElementById('chatMessages');
        const typingDiv = document.createElement('div');
        typingDiv.className = 'chat-message bot typing-indicator active';
        typingDiv.id = 'typingIndicator';
        typingDiv.innerHTML = `
            <div class="message-avatar bot">🤖</div>
            <div class="message-content bot">
                <div class="typing-dot"></div>
                <div class="typing-dot"></div>
                <div class="typing-dot"></div>
            </div>
        `;
        messagesContainer.appendChild(typingDiv);
        messagesContainer.scrollTop = messagesContainer.scrollHeight;
    }

    hideTypingIndicator() {
        const indicator = document.getElementById('typingIndicator');
        if (indicator) {
            indicator.remove();
        }
    }

    escapeHtml(text) {
        const div = document.createElement('div');
        div.textContent = text;
        return div.innerHTML;
    }
}

document.addEventListener('DOMContentLoaded', () => {
    new ChatBot();
});