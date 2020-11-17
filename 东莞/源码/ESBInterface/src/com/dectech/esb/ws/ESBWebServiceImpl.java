package com.dectech.esb.ws;

import javax.jws.WebMethod;
import javax.jws.WebService;

import org.apache.commons.logging.Log;
import org.apache.commons.logging.LogFactory;

import com.dc.eai.data.CompositeData;
import com.dc.eai.data.Field;
import com.dc.eai.data.FieldAttr;
import com.dc.eai.data.FieldType;
import com.dcfs.esb.client.ESBClient;
import com.dcfs.esb.client.exception.TimeoutException;


@WebService(serviceName = "ESBWebService", portName = "ESBServiceInstance", name = "ESBWebService", targetNamespace = "http://www.dectech.com")
public class ESBWebServiceImpl {
	private static Log log = LogFactory.getLog(ESBWebServiceImpl.class);
	
	@WebMethod
	public String DoActionString(String strAction) {
		if (log.isDebugEnabled()) {
			log.debug("Application Action[" + strAction + "]");
		}
		//这一步不能少，这是指定密钥文件所在路径(密钥文件不必一定放在工程的src下，也可以放在工程外面)
		//System.setProperty(ClientConfig.FILEPATH, "E:/DRC/EAI_Home/");
		//读取配置文件中的EAI_Home
		
		CompositeData requestCD = new CompositeData();
		
		//==============================Sys-header Start===========================================
		CompositeData sysHead_struct = new CompositeData();
		
		Field service_code = new Field(new FieldAttr(FieldType.FIELD_STRING, 30));
		Field service_scene = new Field(new FieldAttr(FieldType.FIELD_STRING, 2));
		Field service_version = new Field(new FieldAttr(FieldType.FIELD_STRING, 2));
		Field source_branch_no = new Field(new FieldAttr(FieldType.FIELD_STRING, 20));
		Field user_id = new Field(new FieldAttr(FieldType.FIELD_STRING, 30));
		Field tran_date = new Field(new FieldAttr(FieldType.FIELD_STRING, 8));
		Field tran_timestamp = new Field(new FieldAttr(FieldType.FIELD_STRING, 9));
		Field message_code = new Field(new FieldAttr(FieldType.FIELD_STRING, 6));
		Field source_type = new Field(new FieldAttr(FieldType.FIELD_STRING, 2));
		Field consumer_id = new Field(new FieldAttr(FieldType.FIELD_STRING, 6));
		Field consumer_seq_no = new Field(new FieldAttr(FieldType.FIELD_STRING, 52));
		
		Field credit_tran_code = new Field(new FieldAttr(FieldType.FIELD_STRING, 20));
		Field card_no = new Field(new FieldAttr(FieldType.FIELD_STRING, 19));
		Field sys_follow_seq_no = new Field(new FieldAttr(FieldType.FIELD_STRING, 52));
		Field tran_index_no = new Field(new FieldAttr(FieldType.FIELD_STRING, 52));
		
		//赋值
		service_code.setValue("07002000005");
		service_scene.setValue("01");
		service_version.setValue("02");
		source_branch_no.setValue("80101");
		user_id.setValue("81201");
		String date =  new java.text.SimpleDateFormat("yyyyMMdd").format(new java.util.Date());
		tran_date.setValue(date);
		String time = new java.text.SimpleDateFormat("HHmmssSSS").format(new java.util.Date());
		tran_timestamp.setValue(time);
		message_code.setValue("0103");
		source_type.setValue("09");
		consumer_id.setValue("400015");
		
		String seq = "400015" + date + "000" + time;
		consumer_seq_no.setValue(seq);
		
		credit_tran_code.setValue("");
		card_no.setValue("");
		String sysfollowseqno=new java.text.SimpleDateFormat("HHmmss").format(new java.util.Date());
		sys_follow_seq_no.setValue(sysfollowseqno);
		tran_index_no.setValue("");
		
		
		//加入到系统头内
		sysHead_struct.addField("SERVICE_CODE", service_code);
		sysHead_struct.addField("SERVICE_SCENE", service_scene);
		sysHead_struct.addField("SERVICE_VERSION", service_version);
		sysHead_struct.addField("SOURCE_BRANCH_NO", source_branch_no);
		sysHead_struct.addField("USER_ID", user_id);
		sysHead_struct.addField("TRAN_DATE", tran_date);
		sysHead_struct.addField("TRAN_TIMESTAMP", tran_timestamp);
		sysHead_struct.addField("MESSAGE_CODE", message_code);
		sysHead_struct.addField("SOURCE_TYPE", source_type);
		sysHead_struct.addField("CONSUMER_ID", consumer_id);
		sysHead_struct.addField("CONSUMER_SEQ_NO", consumer_seq_no);		

		//==============================Sys-header End=============================================
		//==============================App-header Start===========================================
		//CompositeData appHead_struct = new CompositeData();
		//Field buss_seq_no = new Field(new FieldAttr(FieldType.FIELD_STRING, 52));
		//buss_seq_no.setValue("");
		//appHead_struct.addField("BUSS_SEQ_NO", buss_seq_no);
		//==============================App-header End=============================================
		//==============================local-header Start=========================================
		//CompositeData localHead_struct = new CompositeData();
	    //==============================local-header End===========================================
		//==============================body Start=================================================
		CompositeData body_struct = new CompositeData();
		Field instinct_data = new Field(new FieldAttr(FieldType.FIELD_STRING, 3000));
		Field handle_code = new Field(new FieldAttr(FieldType.FIELD_STRING, 10));
		instinct_data.setValue(strAction);
		handle_code.setValue("920000");
		body_struct.addField("ADD_DATA", instinct_data);
		body_struct.addField("HANDLE_CODE", handle_code);
		body_struct.addField("SYS_FOLLOW_SEQ_NO", sys_follow_seq_no);
		body_struct.addField("CREDIT_TRAN_CODE", credit_tran_code);
		body_struct.addField("CARD_NO", card_no);
		body_struct.addField("TRAN_INDEX_NO", tran_index_no);
	    //==============================body End===================================================
		
		requestCD.addStruct("SYS_HEAD", sysHead_struct);
		//requestCD.addStruct("APP_HEAD", appHead_struct);
		//requestCD.addStruct("LOCAL_HEAD", localHead_struct);
		requestCD.addStruct("BODY", body_struct);
		
		try {
			
			CompositeData responesCD = ESBClient.request(requestCD);
			String ret_status = responesCD.getStruct("SYS_HEAD").getField("RET_STATUS").strValue();
			String ret_code = responesCD.getStruct("SYS_HEAD").getArray("RET").getStruct(0).getField("RET_CODE").strValue();
			//String ret_msg = responesCD.getStruct("SYS_HEAD").getArray("RET").getStruct(0).getField("RET_MSG").strValue();
			
			if ("S".equalsIgnoreCase(ret_status) && "000000".equalsIgnoreCase(ret_code)) {
				//交易成功的处理
				return "SUCCESS";
			}
		} catch (TimeoutException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}		
		
		return "FAILED";
	}
}
