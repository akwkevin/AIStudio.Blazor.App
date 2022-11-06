一、证书制作说明
参考网址：https://www.cnblogs.com/chenxf1117/p/15119692.html
第一步：生成私钥文件.key
openssl genrsa -out aistudio.key 2048

第二步：生成csr证书签名请求文件
openssl req -new -key aistudio.key -out aistudio.csr
注意 Common Name：www.aistudio.com

第三步：生成.Cer证书
openssl x509 -req -days 365 -in aistudio.csr -signkey aistudio.key -out aistudio.cer -extfile http.ext
这里注意： -extfile http.ext ，根目录下新增http.ext文件，并填写以下内容：
_________________________________________________________________________________________________
keyUsage = nonRepudiation, digitalSignature, keyEncipherment
extendedKeyUsage = serverAuth, clientAuth
subjectAltName=@SubjectAlternativeName
[ SubjectAlternativeName ]
DNS.1=www.aistudio.com
_________________________________________________________________________________________________
DNS.1=www.samples.com 这里实际上就是指定主题备用名称，和证书文件中的Common Name域名保持一致。
完了之后，我们可以看下证书信息：
openssl x509  -in aistudio.cer -text -noout

第四步：同样，生成.pfx证书（包含私钥和公钥）
openssl pkcs12 -export -out aistudio.pfx -inkey aistudio.key -in aistudio.cer

密码为aistudio

二、证书信任说明
参考网址：http://user.tnblog.net/hb/article/details/7610

在证书管理器中的Intermediate Certification Authorities和Trusted Root Certification Authorities页签导入aistudio.cer证书即可

三、hosts文件修改，位置C:\WINDOWS\system32\drivers\etc
尾部添加一行，证书上的网址
127.0.0.1 www.aistudio.com
这样在浏览器里就能使用www.aistudio.com这个网址了。
说明：Hosts是一个没有扩展名的系统文件，可以用记事本等工具打开，其作用就是将一些常用的网址域名与其对应的IP地址建立一个关联“数据库”，当用户在浏览器中输入一个需要登录的网址时，系统会首先自动从Hosts文件中寻找对应的IP地址，一旦找到，系统会立即打开对应网页，如果没有找到，则系统再会将网址提交 DNS 域名解析服务器进行IP地址的解析。 简单来说，hosts是一个浏览网页控制文件，可以从源头上控制DSN指向，在浏览网页中起着举足轻重的作用。