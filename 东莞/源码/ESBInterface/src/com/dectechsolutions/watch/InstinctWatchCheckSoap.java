
package com.dectechsolutions.watch;

import javax.jws.WebMethod;
import javax.jws.WebParam;
import javax.jws.WebResult;
import javax.jws.WebService;
import javax.xml.bind.annotation.XmlSeeAlso;
import javax.xml.ws.RequestWrapper;
import javax.xml.ws.ResponseWrapper;


/**
 * This class was generated by the JAX-WS RI.
 * JAX-WS RI 2.1.6 in JDK 6
 * Generated source version: 2.1
 * 
 */
@WebService(name = "InstinctWatchCheckSoap", targetNamespace = "http://dectechsolutions.com/Instinct")
@XmlSeeAlso({
    ObjectFactory.class
})
public interface InstinctWatchCheckSoap {


    /**
     * 
     * @param batchno
     * @return
     *     returns java.lang.String
     */
    @WebMethod(operationName = "GetDataImportingStatus", action = "http://dectechsolutions.com/Instinct/GetDataImportingStatus")
    @WebResult(name = "GetDataImportingStatusResult", targetNamespace = "http://dectechsolutions.com/Instinct")
    @RequestWrapper(localName = "GetDataImportingStatus", targetNamespace = "http://dectechsolutions.com/Instinct", className = "com.dectechsolutions.watch.GetDataImportingStatus")
    @ResponseWrapper(localName = "GetDataImportingStatusResponse", targetNamespace = "http://dectechsolutions.com/Instinct", className = "com.dectechsolutions.watch.GetDataImportingStatusResponse")
    public String getDataImportingStatus(
        @WebParam(name = "batchno", targetNamespace = "http://dectechsolutions.com/Instinct")
        String batchno);

}
