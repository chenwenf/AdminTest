1������ʾ�� ��http://taurus.cyqdata.com
2����Դ��ַ ��https://github.com/cyq1162/Taurus.MVC
3��CYQ.Data ��Դ��ַ: https://github.com/cyq1162/cyqdata

ʹ��ע�����
1�������ļ�˵��
<configuration>
  <appSettings>
    <!--����Ҫ�ĳɣ����������ڵ���Ŀ������dll���ƣ���������׺�����������ö��ŷָ���-->
    <add key="Taurus.Controllers" value="��Ŀ������dll����" />
    <!--ָ������ĺ�׺��Ĭ���޺�׺��������.shtml��-->
    <add key="Taurus.Suffix" value=""/>
	 <!--�Ƿ������������Ĭ��true-->
    <add key="IsAllowCORS" value="true"/>
    <!--·��ģʽ��ֵΪ0,1��2��[Ĭ��Ϊ1]
      ֵΪ0��ƥ��{Action}/{Para}
      ֵΪ1��ƥ��{Controller}/{Action}/{Para}
      ֵΪ2��ƥ��{Module}/{Controller}/{Action}/{Para}-->
    <add key="RouteMode" value="1"/>
	 <!--�Ƿ�����ű��������-->
    <add key="IsAllowCORS" value="true"/>
    <!--ָ��ҳ����ʼ����·��-->
    <add key="DefaultUrl" value="home/index"/>
  </appSettings>
  <system.web>
    <httpModules>
      <!--Taurus IISӦ�ó���أ�����ģʽ�������У����������ã���֮��ע�͵����У�-->
			<add name="Taurus.Core" type="Taurus.Core.UrlRewrite,Taurus.Core" />
    </httpModules>
  </system.web>
  <system.webServer>
    <modules>
      <!--Taurus IISӦ�ó���أ�����ģʽ�������У����������ã���֮��ע�͵����У�-->
      <add name="Taurus.Core" type="Taurus.Core.UrlRewrite,Taurus.Core" />
    </modules>
  </system.webServer>
</configuration>

A����Ŀ��Ҫ�ж�Ӧ��Controller��Ŀ��Ĭ�ϵ����õ���Ŀ���ƣ�Taurus.Controllers
B����������ģʽע�͵�Taurus.Core���õ�����һ����

������־��
1������CYQ.Data �汾��
2��Controllerȡ��ajaxResult���ԣ�����Write��������������ݡ�
V2.1.1.1
1������Token��֤���ԣ���DefautController�п��Զ��岢ʵ��Token��֤����2016-11-16����public static bool CheckToken(IController controller, string methodName){}��
2��Controller��ǿWrite���� (2016-11-30)
3��Controller����GetEntity<T>()���� (2016-11-30)
V2.2.0.0
1��Controller����GetJson()���� (2016-12-07)
2�����ӿ���֧��(2016-12-07)
3����ǿGetEntity<T>()����(2016-12-07)

V2.2.2.2
1������CYQ.Data(2017-02-04)
2������Session֧��(2017-02-04)

V2.2.2.5 (2017-02-28)
1������CYQ.Data
2���Ż�3�������Session�����򡢱��� ������ĳЩ��������쳣������


V2.2.2.6 (2017-03-27)
1��ÿ��Controller���������ȴ���CheckToken���������ڣ���Ŵ���DefautController�е�CheckTokenȫ�ַ���

V2.2.2.7 (2017-04-01)
1��Сϸ���Ż�������û�����ã�itemref)���Ƴ��ڵ㣨ԭ��ֻ��ȥ�����ԣ�,ͨ����Сϸ�ڣ�������master.html�оͿ������ȷ��ö�����ã�(2017-04-01)
2������LogicBase�Ĳ���ȱ�ٵ����⡣(2017-04-17)
3��DefautController����ȫ��BeforeInvoke����������������ȫ�ִ���(2017-04-17)
public static bool BeforeInvoke(IController controller, string methodName)
4������HttpGet��HttpPost����(2017-04-17)

V2.2.2.8 (2017-04-18)
1��Query�����������ط���������ȡ��Para�е�ֵ��(2017-04-18)
2������DefaultUrl���������Ĭ����ʼ����·����(2017-04-29)

V2.2.3.1(2017-05-15)
1������CheckFormat������֧�ֲ���Ϊ�ջ�������֤��

V2.2.3.3(2017-06-16)
1�����ӷ���������֧��(���ݳ���webapi��ʹ�÷���)
2��CYQ.Dataͬʱ������V5.7.7.4

V2.2.3.4(2017-07-05,2017-10-22)
1 :��ǿ������
2������Query<T>(aaa,defaultValue)��Ĭ��ȡ��ȡֵ˳�����⡣
3������EndInvode�¼���BenginInvode���¼�ִ��˳�������
4��CYQ.Dataͬʱ������V5.7.8.3

V2.2.3.5(2017-04-19)
1��֧��Controller�ֲ��ڲ�ͬ��dll�У�Taurus.Controllers���������������ŷָ�����
2��֧��Controller���μ̳У�A��B   B��Taurus.Core.Controller��