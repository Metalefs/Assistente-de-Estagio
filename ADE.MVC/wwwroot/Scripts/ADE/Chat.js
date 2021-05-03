class ChatMessage {

	constructor(chat) {
		this.chat = chat;

	}

	addMessage(mensagem, origem) {
		
		let id;
		let hora = this.chat.HorarioAtual();
		if (origem === this.chat.enumUser) { this.chat.TotalUserMessages++ }
		if (origem === this.chat.enumOther) {
			id = 'mensagemOther' + this.chat.TotalMessages; this.chat.TotalMessages++;
		}
		else if (origem !== this.chat.enumSystem) id = 'mensagemUser' + this.chat.TotalMessages;
		else id = this.enumSystem + this.chat.TotalMessages;


		this.chat.ListaMensagens.push(this);
		this.chat.AdicionarMensagemDB(this);
		if (mensagem.length > 40 || mensagem !== undefined) {
			if (addBreakWord(mensagem)[1] > 40) {
				mensagem = this.addBreakWord(mensagem)[0];
			}
		}
		if (origem === this.chat.enumOther) {
			return `
				<div class="from-${origem}">
					<div class="message-inner">
						<p id="`+ id + `" class="msg-System"></p>
						<p class='chat-text'>${mensagem}</p>
					</div><br>
					<div class="text-muted hora-envio-other">System: `+ this.chat.DataAtual() + "  -  " + hora + `</div>
					<br>
				</div>
            `;
		} else if (origem === this.chat.enumSystem) {
			return `
                <div class="from-${this.from}">
                    <div class="message-inner">
                        <p class='chat-text'>${mensagem}</p>
                    </div><br>
                    <div class="text-muted hora-envio-other">System: `+ this.chat.DataAtual() + "  -  " + hora + `</div>
                    <br>
                </div>
                `;
		} else if (origem === this.chat.enumUser) {
			return `
                <div class="from-${origem}">
                    <div class="message-inner">
                        <p class='chat-text'>${mensagem}</p>
                    </div><br>
                    <div class="text-muted hora-envio-user">Você: `+ this.chat.DataAtual() + "  -  " + hora + `</div>
                    <br>
                </div>
                `;
		} else {
			return `
                <div class="from-${origem}">
                    <div class="message-inner">
                        <p class='chat-text'>${mensagem}</p>
                    </div><br>
                    <div class="text-muted hora-envio-error">Erro: `+ hora + `</div>
                    <br>
                </div>
                `;
		}
		function addBreakWord (mensagem) {
			let Wbreak = mensagem.slice(54);
			let brMensagem = mensagem.slice(0, 54) + Wbreak;
			return [brMensagem, Wbreak.length];
		}
	}

}

export class Chat{

	#chatColumn = document.getElementById("chat-column-div");
	#textInput = document.getElementById('textInput');
	#ButtonInput = $("#ButtonInput");
	#chat = document.getElementById('conversation');
	#chatInner = document.getElementById('chat');
	#inputOutline = document.getElementById('chat-inputOutline');
	#messages = document.getElementById("messages");
	#loading = document.getElementById('loading');
	#chatWindow = document.getElementsByClassName('chat-column')[0];
	#closeChat = document.getElementById('close-btn');
	#openChat = document.getElementById('open-chat');
	#BeginChat = document.getElementById('begin-chat');
	#connAnimation = document.getElementById('conn-animation');
	#connDiv = document.getElementById('conn-div');
	#InfoChat = document.getElementById('informacoes-bot');
	#PerfilAssistente = document.getElementById('perfil-assistente-chat');
	#MessageAlert = document.getElementById('message-alert');
	#ExtraActions = document.getElementById('extra-options');
	#getToBottom = document.getElementById('get-to-bottom');
	#getToTop = document.getElementById('get-to-top');
	#selectMatch = document.getElementById('select-match');
	#EmpresaName = document.getElementById('Empresa-name');
	#EmpresaNameInput = document.getElementById("Empresa-name-input");
	#OutroName = document.getElementById('Outro-name');
	#OutroNameInput = document.getElementById("Outro-name-input");
	#IDForm = document.getElementById('user-identification');
	#alerta = document.getElementById('alerta');
	#disclaimer = document.getElementById('disclaimer');
	#saveChat = document.getElementById('save-chat');
	#Chamada = document.getElementById('chamada-text');
	#mensagemErro = 'Ocorreu um erro na conexão! Por favor tente mais tarde, ou nos contate pelo email suporte@assistentedeestagio.com';

	#enumUsuario = 'Usuario';
	#enumSistema = 'Sistema';
	#enumOther = 'watson';
	#enumUser = 'user';
	#enumSystem = 'system';
	#enumEntidade = '';
	#enumIntencao = '';

	#TotalMessages = 0;
	#TotalOtherMessages = 0;
	#TotalUserMessages = 0;
	#SegundosUltimaMensagem = 0;
	#EmailCounter = 0;
	#ListaMensagens = [];
	#CHATVersion = "1.1.2";
	#GUID;
	#ListaMensagemsAutoComplete = [];

	constructor(userName, userEmail, id) {
		this.Endpoint = "Chat";
		let self = this;
		this.URIS = {
			UriConversa: "/Chat/ObterHistoricoConversaComUsuario",
			UriSalvarMensagemDB: "/Chat/SalvarMensagem"
		};
		this.UserData = { UserName: userName, UserEmail: userEmail, Id: id };
		this.setup();
		this.ChatMessage = new ChatMessage(this);
		this.UsuarioDestino = "";
	}
	SetUsuarioDestino(value) {
		this.UsuarioDestino = value;
	}
	setup() {
		this.GUID = this.fGUID();
		var WritingAnimationAfterStyle = document.createElement("style");
		WritingAnimationAfterStyle.innerHTML =
			`.classAnimation::after{font-size:8pt;font-style:italic;font-family:Roboto,sans-serif;letter-spacing:-1px;content:"O Assistente está escrevendo";animation:appear 1s ease-in-out infinite}}`;
		document.head.appendChild(WritingAnimationAfterStyle);
		this.setGuid();

		this.chatColumn = document.getElementById("chat-column-div");
		this.textInput = document.getElementById('textInput');
		this.ButtonInput = $("#ButtonInput");
		this.chat = document.getElementById('conversation');
		this.chatInner = document.getElementById('chat');
		this.inputOutline = document.getElementById('chat-inputOutline');
		this.messages = document.getElementById("messages");
		this.loading = document.getElementById('loading');
		this.chatWindow = document.getElementsByClassName('chat-column')[0];
		this.closeChat = document.getElementById('close-btn');
		this.openChat = document.getElementById('open-chat');
		this.BeginChat = document.getElementById('begin-chat');
		this.connAnimation = document.getElementById('conn-animation');
		this.connDiv = document.getElementById('conn-div');
		this.InfoChat = document.getElementById('informacoes-bot');
		this.PerfilAssistente = document.getElementById('perfil-assistente-chat');
		this.MessageAlert = document.getElementById('message-alert');
		this.ExtraActions = document.getElementById('extra-options');
		this.getToBottom = document.getElementById('get-to-bottom');
		this.getToTop = document.getElementById('get-to-top');
		this.selectMatch = document.getElementById('select-match');
		this.EmpresaName = document.getElementById('Empresa-name');
		this.EmpresaNameInput = document.getElementById("Empresa-name-input");
		this.OutroName = document.getElementById('Outro-name');
		this.OutroNameInput = document.getElementById("Outro-name-input");
		this.IDForm = document.getElementById('user-identification');
		this.alerta = document.getElementById('alerta');
		this.disclaimer = document.getElementById('disclaimer');
		this.saveChat = document.getElementById('save-chat');
		this.Chamada = document.getElementById('chamada-text');

		this.enumUsuario = 'Usuario';
		this.enumSistema = 'Sistema';
		this.enumOther = 'watson';
		this.enumUser = 'user';
		this.enumSystem = 'system';
		this.enumEntidade = '';
		this.enumIntencao = '';

		this.TotalMessages = 0;
		this.TotalOtherMessages = 0;
		this.TotalUserMessages = 0;
		this.SegundosUltimaMensagem = 0;
		this.EmailCounter = 0;
		this.ListaMensagens = [];
		this.CHATVersion = "1.1.2";
		this.GUID;

		//window.addEventListener('resize', function () {
		//	this.chatColumn.style.right = "0px !important";
		//});

		this.textInput.addEventListener('keydown', (event) => {
			var d = new Date();
			if (event.keyCode === 13 && this.textInput.value && (d.getTime() - this.SegundosUltimaMensagem / 1000) > 2) {
				// Send the user message
				this.RecuperarMensagensConversaAtual(this.textInput.value);
				const template = this.ChatMessage.addMessage(this.textInput.value, 'user');
				this.InsertTemplateInTheChat(template.addMessage());
				// Clear input box for further messages
				this.textInput.value = '';
				this.SegundosUltimaMensagem = d.getTime();
			} else if (event.keyCode === 13 && this.textInput.value && d.getSeconds() < this.SegundosUltimaMensagem + 2) {
				const template = this.ChatMessage.addMessage("Favor esperar pela resposta.", this.enumSystem);
				this.InsertTemplateInTheChat(template.addMessage());
			}
		});
		this.ButtonInput.click(() => {
			var d = new Date();
			this.SegundosUltimaMensagem = d.getSeconds();
			if (d.getSeconds() < this.SegundosUltimaMensagem + 2) {
				if (this.textInput.value && this.ListaMensagens.length > 0) {
					this.PlayWrittingAnimation();
					// Send the user message
					this.RecuperarMensagensConversaAtual(this.textInput.value);
					const template = this.ChatMessage.addMessage(this.textInput.value, 'user');
					this.InsertTemplateInTheChat(template.addMessage());
					// Clear input box for further messages
					this.textInput.value = '';
				}
			} else {
				const template = this.ChatMessage.addMessage("Favor esperar pela resposta.", this.enumSystem);
				this.InsertTemplateInTheChat(template.addMessage());
			}
		});

		this.getToBottom.addEventListener('click', () => {
			this.FocusChatOnLastMessage();
		});
		this.getToTop.addEventListener('click', () => {
			this.FocusChatOnFirstMessage();
		});
		this.dragElement(document.getElementById("chat-column-div"));
	}

	increaseFontSize(id, increaseFactor){
		newFontSize = this.incSize($('p.chat-text').css("font-size"), 5, 5, 30);
		$('p.chat-text').css("font-size", newFontSize);
		$('p.chat-text').css("line-height", newFontSize);
	}
	incSize(currentSize, incr, min, max) {
		fSize = (parseFloat(currentSize) + incr) % max + min;
		return (fSize) + 'px';
	}
	sleep(ms) {
		return new Promise(resolve => setTimeout(resolve, ms));
	}
	addZero(i) {
		if (i < 10) {
			i = "0" + i;
		}
		return i;
	}
	convertBool(bool) {
		if (bool === false)
			return 0;
		else return 1
	};
	DataAtual() {
		let horario = new Date();
		var mes = horario.getMonth() + 1;
		var dia = horario.getUTCDate();
		var ano = horario.getFullYear();
		let data = mes + "/" + dia + "/" + ano;
		return data;
	}
	HorarioAtual() {
		let horario = new Date();
		var h = this.addZero(horario.getHours());
		var m = this.addZero(horario.getMinutes());
		var s = this.addZero(horario.getSeconds());
		let hora = h + ":" + m + ":" + s;
		return hora;
	}
	DiaAtual() {
		let dia = new Date();
		let dias = ['Domingo', 'Segunda-feira', 'Terça-feira', 'Quarta-feira', 'Quinta-feira', 'Sexta-feira', 'Sábado'];
		diaAtual = dias[dia.getUTCDay()];
		return diaAtual;
	}
	fGUID = function () {
		if (sessionStorage.getItem("guid-chat")) {
			return sessionStorage.getItem("guid-chat");
		}
		else {
			return '_' + (Date.now().toString(36) + Math.random().toString(36).substr(2, 5)).toUpperCase();
		}
	};

	setGuid() {
		sessionStorage.setItem("guid-chat", this.GUID);
	}
	
	CheckInternalErrors = (response) => {
		// if (typeof response.code != undefined) {
		//	return true;
		//} else {
		//	return false;
		//}
		return false;
	};
	ThrowErrorMessage = () => {
		const ErrorTemplate = this.ChatMessage.addMessage(mensagemErro, this.enumOther);
		// Delaying deploy of response, for better UX 
		this.PlayWrittingAnimation();
		sleep(1000).then(x => { //
			this.InsertTemplateInTheChat(ErrorTemplate.addMessage());
			this.StopWrittingAnimation();
			this.connDiv.className = "connAnimation";
		});
	};
	async ChamadaTeste(text) {
		let firstResponse;
		let end = 0;
		try {
			firstResponse = await (await fetch(this.URIS.UriConversa, {
				method: 'POST',
				headers: { 'Content-Type': 'application/json' },
				body: JSON.stringify({
					text,
					context,
				}),
				error: function (xhr, textStatus, error) {
					console.log(xhr.statusText);
					console.log(textStatus);
					console.log(error);
				}
			}));
		} catch (err) {
			console.log(err);
			end = 1;
		}
		return end;
	}

	InsertTemplateInTheChat = (template) => {
		const div = document.createElement('div');
		div.innerHTML = template;
		this.AdicionarMensagemDB(template);
		this.FocusChatOnLastMessage();
		this.connAnimation.className = "hide-chat";
		this.connDiv.className = "hide-chat";
		this.TotalMessages++;
		this.MessageAlert.innerHTML = TotalMessages;
		sessionStorage.setItem("mensagens-chat-" + this.UsuarioDestino, JSON.stringify(this.ListaMensagens));
		sessionStorage.setItem("inputs-não-reconhecidos", JSON.stringify(this.listaInputsNaoReconhecidos));
	};
	
	RecuperarMensagensConversaAtual = async (text = '') => {
		//if (this.TotalMessages <= 0) {
		//	try {
		//		if (await this.ChamadaTeste("") !== 0) {
		//			this.ThrowErrorMessage();
		//		} else {
		//			sleep(1000).then(x => {
		//				this.StopWrittingAnimation();
		//			});
		//		}
		//	}
		//	catch (err) {
		//		if (err) {
		//			console.error(err);
		//		}
		//	}
		//}
		//else {
			//console.log(
				//text
			//);
		let response = await $.ajax({
			type: "get",
			url: this.URIS.UriConversa,
			data: { idUsuario: this.UsuarioDestino },
				success: function (data) {
					console.log(data);
				}
			});
			let IsError = this.CheckInternalErrors(response);
			console.log("Err Status: ", IsError);
			if (IsError === false) {
				console.log("response: ", response);
				try {
					const template = this.ChatMessage.addMessage(response.mensagem.texto, response.mensagem.origem);
					this.InsertTemplateInTheChat(template.addMessage()); 
				}
				catch (err) {
					this.CheckEmptyResponseAndReturn(response);
				}
				this.FocusChatOnLastMessage();
			}
		//}
	};
	CheckEmptyResponseAndReturn = (response) => {
		try {
			if (response.hasOwnProperty("mensagem") && !response.mensagem.autor.hasOwnProperty("nome")) {
				let template = this.ChatMessage.addMessage("Ocorreu um erro na conversa", this.enumSystem);
				this.InsertTemplateInTheChat(template.addMessage());
				this.connAnimation.className = "connAnimation";
			} else if (response.mensagem.texto !== null) {
				this.connAnimation.className = "hide";
			}
		} catch (err) {
			throw err;
		}
	};

	//Create form to get email data
	createEmailForm = (userInfo) => {
		const nome = this.UserData.UserName;
		var EmailForm = document.createElement("div");
		EmailForm.setAttribute("class", "email-form");
		this.EmailCounter++;
		var EmailFormHTML = `
		<div class="from-watson email-form">
			<div class="panel panel-default message-inner">
				<div class="panel-body email-form2">
					<h5 class="input-group card-header" for="subject-input`+ this.EmailCounter + `">${userInfo}</h5>
					<select class="form-control" type="text" title="Assunto" placeholder="Assunto da mensagem" id="subject-input`+ EmailCounter + `">
						<option value="Feedback">Feedback</option>
						<option value="Contato">Contato</option>
					</select>
					<label class="input-group" title="Corpo da mensagem" for="subject-message`+ this.EmailCounter + `">Escreva a mensagem:</label>
					<textarea class="form-control" style="width:356px"type="text" title="Corpo" placeholder="Corpo da mensagem" id="subject-message`+ EmailCounter + `" col="5"></textarea>
					<button class="btn btn-primary" onclick="`+ this.Chatname + `.btnEnviarEmail(` + this.EmailCounter + `)">Enviar</button>
					<button class="btn btn-danger" onclick="`+ this.Chatname + `.btnCancelarEmail()">Cancelar</button>
				</div>
			</div>
		</div>
		`;
		this.InsertTemplateInTheChat(EmailFormHTML);
		this.EmailFormOpen = true;
	}
	//SENDING EMAIL
	SendEmail = (emailDataArray) => {
		if (UserData.UserName === '' || UserData.UserName === null || UserData.UserName === undefined) {
			UserInfo.UserName = "Nome não informado";
		}
		const EmailRequest = {
			"Nome": UserInfo.UserName,
			"Email": UserInfo.UserEmail,
			"Mensagem": EmailData.text
		}
		$.ajax({
			type: "POST",
			xhrFields: {
				withCredentials: true
			},
			url: URIS.UriEmail,
			data: EmailRequest,
			success: (data) => {
				this.EnviouEmail = true;
			},
			error: (err) => {
				console.log(err);
			}
		});
	}
	MensagemNaoEntendi = (response) => {
		return response + "? hum..., não entendi";
	}
	// Get current time and greet accordingly
	CheckCurrentTimeAndGreet = (response) => { //switch Time
		var CurrentTime = new Date().toLocaleTimeString('en-US', { hour12: false, hour: "numeric", minute: "numeric" });
		var Time = this.parseInt(CurrentTime.replace(":", ""));
		
			
		if (Time < 1200) {
			const templateDia = this.ChatMessage.addMessage("Bom dia, como posso ajudar?", this.enumOther);
			sleep(500).then(x => {
				this.InsertTemplateInTheChat(templateDia.addMessage());
				console.log("Bom dia");
			});
		}
		if (Time > 1200 && Time < 1800) {
			const templateTarde = this.ChatMessage.addMessage("Boa tarde, como posso ajudar?", this.enumOther);
			sleep(500).then(x => {
				this.InsertTemplateInTheChat(templateTarde.addMessage());
				console.log("Boa tarde");
			});
		}
		if (Time > 1800) {
			const templateNoite = this.ChatMessage.addMessage("Boa noite, como posso ajudar?", this.enumOther);
			sleep(500).then(x => {
				this.InsertTemplateInTheChat(templateNoite.addMessage());
				console.log(Time > 1800);
			});
		}			
	}
	AdicionarMensagemDB = (mensagem) => {
		let Mensagem = {
			IdUsuarioRemetente: this.UserData.Id,
			IdUsuarioDestino: this.UsuarioDestino,
			Conteudo: mensagem
		};
		try {
			$.ajax({
				type: "post",
				url: this.URIS.UriSalvarMensagemDB,
				data: Mensagem,
				success: () => {
					console.log("enviado");
				}
			});
		} catch (err) {
			console.log(err);
			let template = this.ChatMessage.addMessage("Ocorreu um erro na conversa", this.enumSystem);
			this.InsertTemplateInTheChat(template);
			this.connAnimation.className = "connAnimation";
		}
	};
	RecuperarMensagens = () => {
		let userGUID = $("#btnTempRecuperarMsg").val();
		$.ajax({
			type: "get",
			url: this.URIS.UriRecuperarConversa,
			data: { GUID: userGUID },
			success: (data) => {
				console.log("Conversa recuperada");
				console.log(data);
			}
		});
		$("#TempRecuperarMsg").remove();
	}
	FocusChatOnLastMessage = () => {
		if (ListaMensagens.length > 1) {
			let chatList = document.getElementById("chat");
			console.log('focusing chat');
			chatList.scroll({ top: chatList.scrollHeight, behavior: 'smooth' });
			document.querySelector('.message-inner').scrollBy({
				behavior: 'smooth'
			});
			//getToTop.className = "get-to-top-active";
		}
	}
	FocusChatOnFirstMessage = () => {
		let chatList = document.getElementById("chat");
		chatList.scroll({ top: 0, behavior: 'smooth' });
		document.querySelector('.message-inner').scrollBy({
			behavior: 'smooth'
		});
	}
	/*window.addEventListener('mousewheel', () =>{
		getToTop.className = "get-to-top-disabled";
	});*/
	PlayWrittingAnimation = () => {
		this.loading.classList.add("classAnimation");
	}
	StopWrittingAnimation = () => {
		this.loading.classList.remove("classAnimation");
	}
	
	StartChamada = (mensagem) => {
		sleep(1200).then(x => {
		});
	}
	autoSendMsg (evt, vaga) {
		var d = new Date();
		this.SegundosUltimaMensagem = d.getSeconds();
		if (d.getSeconds() < this.SegundosUltimaMensagem + 2) {
			this.RecuperarMensagensConversaAtual(evt);
			const template = this.ChatMessage.addMessage(evt, 'user');
			this.InsertTemplateInTheChat(template.addMessage());
		} else {
			const template = this.ChatMessage.addMessage("Favor esperar pela resposta.", enumSystem);
			this.InsertTemplateInTheChat(template.addMessage());
		}
	};
	btnEnviarEmail(num){
		this.SendEmail(num);
	}
	
	ReiniciarConversaChat = () => {
		sessionStorage.setItem("mensagens-chat", "");
		this.TotalMessages = 0;
		this.TotalOtherMessages = 0;
		this.TotalUserMessages = 0;
		$("#messages").empty();
		this.StartChamada();
	}
	
	dragElement(elmnt) {
		var pos1 = 0, pos2 = 0, pos3 = 0, pos4 = 0, chatbox = 0;
		if (document.getElementById(elmnt.id + "header")) {
			// if present, the header is where you move the DIV from:
			document.getElementById(elmnt.id + "header").onmousedown = dragMouseDown;
		} else {
			// otherwise, move the DIV from anywhere inside the DIV: 
			elmnt.onclick = dragMouseDown;
		}
		function dragMouseDown(e) {
			e = e || window.event;
			e.preventDefault();
			// get the mouse cursor position at startup:
			pos3 = e.clientX;
			pos4 = e.clientY;
			document.onmouseup = closeDragElement;
			// call a function whenever the cursor moves:
			document.onmousemove = elementDrag;
		}
		function elementDrag(e) {
			e = e || window.event;
			e.preventDefault();
			// calculate the new cursor position:
			pos1 = pos3 - e.clientX;
			pos2 = pos4 - e.clientY;
			pos3 = e.clientX;
			pos4 = e.clientY;
			// set the element's new position:
			chatbox = pos3;
			if (window.innerWidth > chatbox || chatbox > 0) {
				if (window.innerWidth > 1200) {
					if (e.clientX + window.innerWidth > window.innerWidth + 300 && e.clientX + 300 < window.innerWidth) {
						elmnt.style.left = (elmnt.offsetLeft - pos1) + "px";
						elmnt.style.bottom = "0px";
					} else {
						elmnt.style.left = "inherit";
					}
				}
			}
		}
		let checkChatPosition = () => {
			if (window.innerWidth < elmnt.style.left || elmnt.style.left < 0) {
				document.getElementById("chat-column-div").style.left = "inherit";
			}
		}
		setInterval(checkChatPosition, 3000);
		function closeDragElement() {
			// stop moving when mouse button is released:
			document.onmouseup = null;
			document.onmousemove = null;
		}
	}
}