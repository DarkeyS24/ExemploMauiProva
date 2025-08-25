# Ponte Dourada - Aplicativo Móvel

## Sistema de Atendimento para Cuidadores e Pacientes

### 🔐 Funcionalidades de Login

O aplicativo possui um sistema robusto de autenticação com as seguintes características:

#### **Login com Dupla Autenticação**
1. **Primeira Camada**: Login e senha do usuário
2. **Segunda Camada**: PIN aleatório de 3 dígitos gerado automaticamente

#### **Funcionalidade "Mantenha-me Conectado"**
- Permite que o usuário permaneça logado mesmo após fechar o aplicativo
- Os dados são armazenados de forma segura no dispositivo
- Na próxima abertura do app, o usuário é automaticamente direcionado para sua tela inicial

### 🔧 Como Reverter a Funcionalidade "Mantenha-me Conectado"

Existem **3 formas** de desabilitar ou reverter a funcionalidade de login automático:

#### **Método 1: Através do Botão de Configurações (Recomendado)**
1. Faça login no aplicativo normalmente
2. Na tela principal (Calendário ou Meus Atendimentos), clique no ícone **⚙️** no canto superior direito
3. Confirme a ação quando perguntado se deseja desabilitar a funcionalidade
4. A partir da próxima abertura do app, será necessário fazer login novamente

#### **Método 2: Não Marcar a Opção na Próxima Vez**
1. Ao fazer login novamente, simplesmente **não marque** a caixa "Mantenha-me conectado"
2. Isso substituirá a configuração anterior

#### **Método 3: Reinstalar o Aplicativo**
1. Desinstale o aplicativo do dispositivo
2. Reinstale novamente
3. Todos os dados de login armazenados serão removidos

### 👥 Usuários de Teste

Para testar o aplicativo, utilize as seguintes credenciais:

#### **Cuidadores**
- **Login**: `joao.silva` | **Senha**: `123456`
- **Login**: `ana.costa` | **Senha**: `123456`

#### **Pacientes**
- **Login**: `maria.santos` | **Senha**: `123456`
- **Login**: `carlos.oliveira` | **Senha**: `123456`

### 🎯 Fluxo de Navegação

#### **Para Cuidadores**
Login → PIN → Calendário de Atendimentos

#### **Para Pacientes**
Login → PIN → Meus Atendimentos

### 🛡️ Segurança

- As senhas são validadas através de um serviço de autenticação
- O PIN de dupla autenticação é gerado aleatoriamente a cada login
- Os dados de "mantenha-me conectado" são armazenados usando `SecureStorage` do .NET MAUI
- Possibilidade de logout e limpeza de dados salvos a qualquer momento

### 🔄 Atualizações Futuras

- Integração com banco de dados real
- Recuperação de senha
- Biometria (digital/face)
- Notificações push
- Sincronização em nuvem

---

**Versão**: 1.0.0  
**Desenvolvido em**: .NET MAUI 8.0  
**Compatibilidade**: Android, iOS, Windows, macOS
