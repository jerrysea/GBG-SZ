package com.dectech.serverimpl;

import java.io.InputStream;
import java.net.URL;
import java.util.Calendar;
import java.util.Date;
import java.util.Properties;
import java.text.*;

import net.sf.json.JSONObject;

import org.apache.commons.logging.Log;
import org.apache.commons.logging.LogFactory;

import com.dc.eai.data.Array;
import com.dc.eai.data.CompositeData;
import com.dc.eai.data.Field;
import com.dc.eai.data.FieldAttr;
import com.dc.eai.data.FieldType;
import com.dcfs.esb.client.ESBClient;
import com.dcfs.esb.client.exception.TimeoutException;
import com.dcfs.esb.server.service.Service;
import com.dectech.util.Helper;
import com.dectechsolutions.instinct.OnlineFraudCheck;
import com.dectechsolutions.instinct.OnlineFraudCheckSoap;
import com.dectechsolutions.watch.InstinctWatchCheck;
import com.dectechsolutions.watch.InstinctWatchCheckSoap;
import com.test.TestServlet;

/**
 * 服务端业务逻辑
 * 
 * @author cloud
 *
 */
public class ServiceImpl implements Service {

	private static Log log = LogFactory.getLog(ServiceImpl.class);
	
	static String FRAUDCHECK="110020000041100";
	static String WATCHCHECK="110030000410800";
	//static String strUrl = "";
	static String FraudCheckWebServiceAddress="";
	static String InstinctWatchCheckAddress="";
	static{
		LoadProperties();
	}
	
	synchronized static public void LoadProperties(){
		 InputStream is = TestServlet.class.getResourceAsStream("/Instinct.properties");
		 Properties prop = new Properties();
		 try{
			 prop.load(is);			 
			 FraudCheckWebServiceAddress= prop.getProperty("FraudCheckWebServiceAddress");
			 InstinctWatchCheckAddress= prop.getProperty("InstinctWatchCheckAddress");			 
		 }catch(Exception e)
		 {
			 System.err.println("不能读取属性文件. " +
				       "请确保db.properties在CLASSPATH指定的路径中");
		 }
	}
	/**
	 * 服务端接收到报文后，处理的代码写在这儿。在esb-server.properties中配置该实现类的路径
	 * service.impl=com.dcfs.server.service.impl.ServiceImpl
	 */
	public CompositeData exec(CompositeData req) {
		if (log.isDebugEnabled()) {
			log.debug("服务端获取到的报文 [" + req + "]");
		}
		//==============================Server Code + Server Scene + Server Version===========================================
		String req_Server_Code=req.getStruct("SYS_HEAD").getField("SERVICE_CODE").strValue();
		String req_Server_Scene=req.getStruct("SYS_HEAD").getField("SERVICE_SCENE").strValue();
		String req_Server_Version=req.getStruct("SYS_HEAD").getField("SERVICE_VERSION").strValue();
		
		String code_scene_version=req_Server_Code+req_Server_Scene+req_Server_Version;
		URL url;
		
		//==================================Return Value=======================================================================
		String instinctOutputData = null;
		String watchoutput = null;
		String watchtrandate=null;
		String watchhandlecode=null;
		String watchhandlemsg=null;
		
		//==============================Declare===========================================
		Field service_code = new Field(new FieldAttr(FieldType.FIELD_STRING, 30));
		Field service_scene = new Field(new FieldAttr(FieldType.FIELD_STRING, 2));
		Field service_version = new Field(new FieldAttr(FieldType.FIELD_STRING, 2));
		Field consumer_id = new Field(new FieldAttr(FieldType.FIELD_STRING, 6));
		Field tran_date = new Field(new FieldAttr(FieldType.FIELD_STRING, 8));
		Field tran_timestamp = new Field(new FieldAttr(FieldType.FIELD_STRING, 9));		
		Field esb_seq_no = new Field(new FieldAttr(FieldType.FIELD_STRING, 52));		
		Field total_rows=new Field(new FieldAttr(FieldType.FIELD_STRING,10));
		Field total_num=new Field(new FieldAttr(FieldType.FIELD_STRING,10));
		Field buss_seq_no = new Field(new FieldAttr(FieldType.FIELD_STRING, 52));
		Field handle_code=new Field(new FieldAttr(FieldType.FIELD_STRING,10));
		Field handle_msg=new Field(new FieldAttr(FieldType.FIELD_STRING,1024));	
		
		Field ret_status = new Field(new FieldAttr(FieldType.FIELD_STRING, 1));
		
		Field ret_code = new Field(new FieldAttr(FieldType.FIELD_STRING, 30));
		Field ret_msg = new Field(new FieldAttr(FieldType.FIELD_STRING, 512));
		
		if(code_scene_version.equals(FRAUDCHECK))//申请欺诈线上检查
		{
			log.debug("申请欺诈线上检查");
			String instinctInputData = req.getStruct("BODY").getField("BIG_DATA_STR1").strValue()
					 + req.getStruct("BODY").getField("BIG_DATA_STR2").strValue();
			log.debug("Request Data:"+instinctInputData);
			try {
				url = new URL(FraudCheckWebServiceAddress);
				OnlineFraudCheck fraudCheck = new OnlineFraudCheck(url);
				OnlineFraudCheckSoap fraudCheckSoap = fraudCheck.getOnlineFraudCheckSoap();
				instinctOutputData = fraudCheckSoap.instinctFraudCheckString(instinctInputData);
				handle_code.setValue("0000000");
				handle_msg.setValue(instinctOutputData);
				log.debug("Output Data:"+instinctOutputData);
			} catch (Exception e) {				
				e.printStackTrace();
				log.error("FRAUDCHECK Error:"+e.getMessage() + "\r\n" + Helper.getTrace(e));
			}
		}
		else if(code_scene_version.equals(WATCHCHECK))//监视
		{
			log.debug("数据集市+预警信息监视...");
			String structbusdate=req.getStruct("BODY").getField("BUSS_DATE").strValue();
			//Date predate=getPreDay(new Date());
			Date date_busdate=strToDate(structbusdate);
			//Date predate=getPreDay(new Date());
			Date predate=getPreDay(date_busdate);
			String bussdate = new java.text.SimpleDateFormat("yyyyMMdd").format(predate);//req.getStruct("BODY").getField("BUSS_DATE").strValue();
			log.debug("pre date:"+bussdate);
			try {
				log.debug("Start to Watch...");
				url = new URL(InstinctWatchCheckAddress);
				InstinctWatchCheck check=new InstinctWatchCheck(url);
		        InstinctWatchCheckSoap checksoap=check.getInstinctWatchCheckSoap();
		        watchoutput=checksoap.getDataImportingStatus(bussdate);
		        
		        log.debug("Watch Output:"+watchoutput);
		        JSONObject jsonObject = JSONObject.fromObject(watchoutput);      
		        
		        watchtrandate=jsonObject.getString("Tran_Date");
		        log.debug("Tran Date:"+watchtrandate);				
				
		        watchhandlecode=jsonObject.getString("HANDLE_CODE");
		        watchhandlecode=watchhandlecode.equals("RUNNING")?"RUN":watchhandlecode;
		        watchhandlecode=watchhandlecode.equals("NOT START")?"CHECK":watchhandlecode;
				
		        watchhandlemsg=jsonObject.getString("HANDLE_MSG");
		        
		        if(watchhandlecode!="SUCCESS" && daysOfTwo(date_busdate,new Date())>0)
		        {
		        	watchhandlecode="FAIL";
		        	watchhandlemsg="The result is still failed, and time has passed at 12:00";
		        }
		        log.debug("HANDLE CODE:"+watchhandlecode);
		        log.debug("HANDLE MSG:"+watchhandlemsg);	
		        handle_code.setValue(watchhandlecode);
				handle_msg.setValue(watchhandlemsg);
				
		        log.debug("End to Watch.");
			} catch (Exception e) {			
				e.printStackTrace();
				log.error("WATCHCHECK Error:"+e.getMessage() + "\r\n" + Helper.getTrace(e));
			}
		}			
		CompositeData responseCD = new CompositeData();
		 
		//==============================Sys-header Start===========================================
		CompositeData sysHead_struct = new CompositeData();
		Array ret_array = new Array();
		CompositeData ret_struct = new CompositeData();		
		
		//赋值
		service_code.setValue(req.getStruct("SYS_HEAD").getField("SERVICE_CODE").strValue());
		service_scene.setValue(req.getStruct("SYS_HEAD").getField("SERVICE_SCENE").strValue());
		service_version.setValue(req.getStruct("SYS_HEAD").getField("SERVICE_VERSION").strValue());
		consumer_id.setValue(req.getStruct("SYS_HEAD").getField("CONSUMER_ID").strValue());
		tran_date.setValue(new java.text.SimpleDateFormat("yyyyMMdd").format(new java.util.Date()));
		tran_timestamp.setValue(new java.text.SimpleDateFormat("HHmmssSSS").format(new java.util.Date()));
		esb_seq_no.setValue(req.getStruct("SYS_HEAD").getField("ESB_SEQ_NO").strValue());
		ret_code.setValue("000000");
		ret_msg.setValue("交易成功");
		ret_status.setValue("S");
		
		//加入到系统头内
		sysHead_struct.addField("SERVICE_CODE", service_code);
		sysHead_struct.addField("SERVICE_SCENE", service_scene);
		sysHead_struct.addField("SERVICE_VERSION", service_version);
		sysHead_struct.addField("CONSUMER_ID", consumer_id);
		sysHead_struct.addField("TRAN_DATE", tran_date);
		sysHead_struct.addField("TRAN_TIMESTAMP", tran_timestamp);
		sysHead_struct.addField("ESB_SEQ_NO", esb_seq_no);		
		sysHead_struct.addField("RET_STATUS", ret_status);
		ret_struct.addField("RET_CODE", ret_code);
		ret_struct.addField("RET_MSG", ret_msg);
		ret_array.addStruct(ret_struct);
		sysHead_struct.addArray("RET", ret_array);
		//==============================Sys-header End=============================================
		//==============================App-header Start===========================================
		CompositeData appHead_struct = new CompositeData();		
		total_rows.setValue("0");
		total_num.setValue("0");
		buss_seq_no.setValue(req.getStruct("APP_HEAD").getField("BUSS_SEQ_NO").strValue());
		appHead_struct.addField("BUSS_SEQ_NO", buss_seq_no);
		appHead_struct.addField("TOTAL_ROWS", total_rows);
		appHead_struct.addField("TOTAL_NUM", total_num);
		//==============================App-header End=============================================
		//==============================body Start=================================================
		CompositeData body_struct = new CompositeData();		
		body_struct.addField("HANDLE_CODE", handle_code);		
		body_struct.addField("HANDLE_MSG", handle_msg);
	    //==============================body End===================================================
		
		responseCD.addStruct("SYS_HEAD", sysHead_struct);
		responseCD.addStruct("APP_HEAD", appHead_struct);		
		responseCD.addStruct("BODY", body_struct);
		
		return responseCD;
	}	
	
	private static Date getPreDay(Date date)
	{
		Calendar calendar=Calendar.getInstance();
		calendar.setTime(date);
		calendar.add(Calendar.DAY_OF_MONTH, -1);
		date=calendar.getTime();
		return date;
	}
	
	public static Date strToDate(String strDate) {
		   SimpleDateFormat formatter = new SimpleDateFormat("yyyyMMdd");
		   ParsePosition pos = new ParsePosition(0);
		   Date strtodate = formatter.parse(strDate, pos);
		   return strtodate;
	}
	
	public static int daysOfTwo(Date fDate, Date oDate) {

	       Calendar aCalendar = Calendar.getInstance();
	       aCalendar.setTime(fDate);
	       int day1 = aCalendar.get(Calendar.DAY_OF_YEAR);
	       aCalendar.setTime(oDate);
	       int day2 = aCalendar.get(Calendar.DAY_OF_YEAR);
	       return day2 - day1;

	}

}
