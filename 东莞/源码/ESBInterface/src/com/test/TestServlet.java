package com.test;

import java.io.IOException;
import java.io.InputStream;
import java.net.URL;
import java.util.Calendar;
import java.util.Date;
import java.util.Properties;

import javax.servlet.ServletException;
import javax.servlet.annotation.WebServlet;
import javax.servlet.http.HttpServlet;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;

import org.apache.commons.logging.Log;
import org.apache.commons.logging.LogFactory;

import net.sf.json.JSONObject;

import com.dectech.esb.ws.ESBWebServiceImpl;
import com.dectech.serverimpl.ServiceImpl;
import com.dectech.util.Helper;
import com.dectechsolutions.instinct.OnlineFraudCheck;
import com.dectechsolutions.instinct.OnlineFraudCheckSoap;
import com.dectechsolutions.watch.*;
import com.dectech.serverimpl.*;

/**
 * Servlet implementation class TestServlet
 */
@WebServlet("/TestServlet")
public class TestServlet extends HttpServlet {
	private static Log log = LogFactory.getLog(TestServlet.class);
	private static final long serialVersionUID = 1L;

	static String strUrl = "";
	static{
		LoadProperties();
	}
	
	synchronized static public void LoadProperties(){
		 InputStream is = TestServlet.class.getResourceAsStream("/Instinct.properties");
		 Properties prop = new Properties();
		 try{
			 prop.load(is);
			 strUrl = prop.getProperty("InstinctWatchCheckAddress");
			 System.out.println(strUrl);
		 }catch(Exception e)
		 {
			 System.err.println("不能读取属性文件. " +
				       "请确保db.properties在CLASSPATH指定的路径中");
		 }
	}
	/**
	 * @see HttpServlet#HttpServlet()
	 */
	public TestServlet() {
		super();
		// TODO Auto-generated constructor stub
	}

	/**
	 * @see HttpServlet#doGet(HttpServletRequest request, HttpServletResponse
	 *      response)
	 */
	protected void doGet(HttpServletRequest request, HttpServletResponse response)
			throws ServletException, IOException {
		// TODO Auto-generated method stub
		

		//URL url = new URL(strUrl);
		//OnlineFraudCheck fraudCheck = new OnlineFraudCheck(url);
		//OnlineFraudCheckSoap fraudCheckSoap = fraudCheck.getOnlineFraudCheckSoap();
		//String result = fraudCheckSoap.instinctFraudCheckString("DRC|CN||2015091600000300040|16/09/2015|CARD||12001|||||||6258881000006709||||CRECUPIC3||||BATIMP|A|||1004586525|A|341301198204010795|A|标普七十|BIAO PU QI SHI||M|01/04/1982|广东|东莞||东莞市道滘镇振兴路北122号||13926836193|13926836193|东莞农村商业银行道滘|广东|东莞||东莞市道滘镇振兴路北122号||||||||||||||||||||MR|G|||||||||||||||||I|||||||U|||||||||||||||");
		//ESBWebServiceImpl imp =new ESBWebServiceImpl();
		
		//String result =imp.DoActionString("this is a test");
		
		//response.getWriter().append(result);		
	}

	/**
	 * @see HttpServlet#doPost(HttpServletRequest request, HttpServletResponse
	 *      response)
	 */
	protected void doPost(HttpServletRequest request, HttpServletResponse response)
			throws ServletException, IOException {
		// TODO Auto-generated method stub
		doGet(request, response);
	}
	
	private static Date getPreDay(Date date)
	{
		Calendar calendar=Calendar.getInstance();
		calendar.setTime(date);
		calendar.add(Calendar.DAY_OF_MONTH, -1);
		date=calendar.getTime();
		return date;
	}

}
