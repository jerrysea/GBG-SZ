
package com.dectechsolutions.instinct;

import javax.xml.bind.annotation.XmlAccessType;
import javax.xml.bind.annotation.XmlAccessorType;
import javax.xml.bind.annotation.XmlElement;
import javax.xml.bind.annotation.XmlRootElement;
import javax.xml.bind.annotation.XmlType;


/**
 * <p>anonymous complex type�� Java �ࡣ
 * 
 * <p>����ģʽƬ��ָ�������ڴ����е�Ԥ�����ݡ�
 * 
 * <pre>
 * &lt;complexType>
 *   &lt;complexContent>
 *     &lt;restriction base="{http://www.w3.org/2001/XMLSchema}anyType">
 *       &lt;sequence>
 *         &lt;element name="InstinctFraudCheck_XMLStringResult" type="{http://www.w3.org/2001/XMLSchema}string" minOccurs="0"/>
 *       &lt;/sequence>
 *     &lt;/restriction>
 *   &lt;/complexContent>
 * &lt;/complexType>
 * </pre>
 * 
 * 
 */
@XmlAccessorType(XmlAccessType.FIELD)
@XmlType(name = "", propOrder = {
    "instinctFraudCheckXMLStringResult"
})
@XmlRootElement(name = "InstinctFraudCheck_XMLStringResponse")
public class InstinctFraudCheckXMLStringResponse {

    @XmlElement(name = "InstinctFraudCheck_XMLStringResult")
    protected String instinctFraudCheckXMLStringResult;

    /**
     * ��ȡinstinctFraudCheckXMLStringResult���Ե�ֵ��
     * 
     * @return
     *     possible object is
     *     {@link String }
     *     
     */
    public String getInstinctFraudCheckXMLStringResult() {
        return instinctFraudCheckXMLStringResult;
    }

    /**
     * ����instinctFraudCheckXMLStringResult���Ե�ֵ��
     * 
     * @param value
     *     allowed object is
     *     {@link String }
     *     
     */
    public void setInstinctFraudCheckXMLStringResult(String value) {
        this.instinctFraudCheckXMLStringResult = value;
    }

}
