<div id="readme" class="Box-body readme blob js-code-block-container">
  <article class="markdown-body entry-content p-3 p-md-6" itemprop="text">
    <p align="center">
      <img alt="GitHub language count" src="https://img.shields.io/github/languages/count/fnascimento00/ReconhecimentoFacialAWS">
      <img alt="GitHub top language" src="https://img.shields.io/github/languages/top/fnascimento00/ReconhecimentoFacialAWS">
      <img alt="GitHub repo size" src="https://img.shields.io/github/repo-size/fnascimento00/ReconhecimentoFacialAWS">
      <img alt="GitHub last commit" src="https://img.shields.io/github/last-commit/fnascimento00/ReconhecimentoFacialAWS">
      <img alt="Github license" src="https://img.shields.io/github/license/fnascimento00/ReconhecimentoFacialAWS">
    </p>
    <h2>:speech_balloon: Projeto</h2>
    <p><strong>ReconhecimentoFacialAWS</strong> é um aplicativo que testa o serviço <a href="https://aws.amazon.com/pt/rekognition/" rel="nofollow">Amazon Rekognition</a>. O aplicativo realiza a autenticação de dois fatores (2FA), sendo a primeira autenticação por meio de um formulário com login e senha e a segunda autenticação por reconhecimento facial. O reconhecimento facial. Para o reconhecimento facial são utilizados os métodos: CompareFacesAsync e DetectFacesAsync do AmazonRekognitionClient.</p>
    <h2>:rocket: Tecnologias</h2>
    <p>Este projeto foi desenvolvido com as seguintes tecnologias:</p>
    <ul>
      <li><a href="https://learn.microsoft.com/pt-br/aspnet/core/mvc/overview?view=aspnetcore-7.0" rel="nofollow">Asp.Net Core MVC</a></li>
      <li><a href="https://aws.amazon.com/pt/rekognition/" rel="nofollow">Amazon Rekognition</a></li>
    </ul>
    <h2>:information_source:</a> Como usar </h2>
    <p>Para clonar e executar este aplicativo, você precisará do <a href="https://dotnet.microsoft.com/pt-br/download/dotnet/7.0" rel="nofollow">.NET 7.0</a>. No terminal execute os seguintes comandos:</p>
    <div class="highlight highlight-source-shell">
      <pre><span class="pl-c"><span class="pl-c">#</span> Clonar o repositório</span>
$ git clone https://github.com/fnascimento00/ReconhecimentoFacialAWS <br/>
<span class="pl-c"><span class="pl-c">#</span> Vá para o repositório</span>
$ <span class="pl-c1">cd</span> ReconhecimentoFacialAWS <br/>

<span class="pl-c"><span class="pl-c">#</span> Edite o arquivo appsettings.json e appsettings.Development.json</span>
$ <span class="pl-c1">#</span> Para utilizar o serviço Amazon Rekognition é necessário criar uma conta na AWS e habilitar o serviço. <br/>Uma vez que o serviço estiver devidamente configurado adicione as seguintes informações nos arquivos appsettings.json e appsettings.Development.json. <br/>
$ <span class="pl-c1">#</span> AccessKey <br/>
$ <span class="pl-c1">#</span> SecretKey <br/>
$ <span class="pl-c1">#</span> Region <br/>

<span class="pl-c"><span class="pl-c">#</span> Compile a aplicação</span>
$ dotnet build <br/>
<span class="pl-c"><span class="pl-c">#</span> Execute a aplicação</span>
$ dotnet run <br/>
<span class="pl-c"><span class="pl-c">#</span> Abra o browser e navegue para https://localhost:7066</span>
<span class="pl-c"><span class="pl-c">#</span> Edite o projeto usando: Visual Studio ou VS Code</span>
</div>
    <h2>:pencil: Licença</h2>
    <p>Este projeto está sob a licença do MIT. Veja o em <a href="https://github.com/fnascimento00/ReconhecimentoFacialAWS/blob/main/LICENSE" rel="nofollow">LICENSE</a> para maiores informações.</p>
    <hr>
    <p>Saiba mais sobre mim: <a href="https://www.linkedin.com/in/flávio-nascimento-8089a232/" rel="nofollow">linkedin</a></p>
  </article>
</div>
