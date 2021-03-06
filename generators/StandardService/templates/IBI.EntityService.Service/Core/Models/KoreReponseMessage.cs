﻿using System.Collections.Generic;
using System.Xml;

// Created by Genie <%= TodaysDate %> by verion <%= Version %>
namespace IBI.<%= Name %>.Service.Models
{
    /// <summary>
    /// Model to handle the response from Kourier Rest Gateway
    /// </summary>
    public class KoreReponseMessage
    {
        #region Constructors

        /// <summary>
        /// Takes in the string that represents the XML and loads the model
        /// </summary>
        /// <param name="response"></param>
        public KoreReponseMessage(string response) : base()
        {
            var reponseDocument = new XmlDocument();
            reponseDocument.LoadXml(response);
            this.LoadXml(reponseDocument);
        }

        /// <summary>
        /// Takes in a XML Document to load the model
        /// </summary>
        /// <param name="xml"></param>
        public KoreReponseMessage(XmlDocument xml) : base()
        {
            this.LoadXml(xml);
        }

        #endregion Constructors

        #region Load the XML

        /// <summary>
        /// Takes the XML document and loads the model based on the attributes
        /// and other nodes
        /// </summary>
        /// <param name="xml"></param>
        private void LoadXml(XmlDocument xml)
        {
            this.Errors = new List<string>();
            var httpCodeNode = xml.SelectSingleNode("/status/http_code");
            if (httpCodeNode != null)
            {
                this.HttpCode = httpCodeNode.InnerText;
            }

            var httpSubCodeNode = xml.SelectSingleNode("/status/http_subcode");
            if (httpSubCodeNode != null)
            {
                this.HttpSubCode = httpSubCodeNode.InnerText;
            }
            var messageNode = xml.SelectSingleNode("/status/message");
            if (messageNode != null)
            {
                this.Message = messageNode.InnerText;
            }

            var recordNode = xml.SelectSingleNode("/status/record");
            if (recordNode != null)
            {
                this.ResourceFile = recordNode.Attributes["ResourceFile"] != null ? recordNode.Attributes["ResourceFile"].Value : null;
                this.ResourceId = recordNode.Attributes["ResourceID"] != null ? recordNode.Attributes["ResourceID"].Value : null;
                this.RemoteId = recordNode.Attributes["RemoteID"] != null ? recordNode.Attributes["RemoteID"].Value : null;
                this.Loaded = recordNode.Attributes["Loaded"] != null ? recordNode.Attributes["Loaded"].Value : null;
                this.Operation = recordNode.Attributes["Operation"] != null ? recordNode.Attributes["Operation"].Value : null;
                this.HRef = recordNode.Attributes["href"] != null ? recordNode.Attributes["href"].Value : null;
                var errors = recordNode.SelectNodes("error");
                foreach (var err in errors)
                {
                    this.Errors.Add(((XmlNode)err).InnerText);
                }
            }
        }

        #endregion Load the XML

        #region Properties

        /// <summary>
        /// list of errors (if any)
        /// </summary>
        public List<string> Errors { get; set; }

        /// <summary>
        /// The url called
        /// </summary>
        public string HRef { get; set; }

        /// <summary>
        /// The main Http response code
        /// </summary>
        public string HttpCode { get; set; }

        /// <summary>
        /// The secondary http repsonse code that advises how the communication to ERP actually went
        /// </summary>
        public string HttpSubCode { get; set; }

        /// <summary>
        /// Loaded
        /// </summary>
        public string Loaded { get; set; }

        /// <summary>
        /// Any return messages
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// What HTTP operation was performed
        /// </summary>
        public string Operation { get; set; }

        /// <summary>
        /// the Id of the object in erp
        /// </summary>
        public string RemoteId { get; set; }

        /// <summary>
        /// The file used on ERP
        /// </summary>
        public string ResourceFile { get; set; }

        /// <summary>
        /// ResourceId
        /// </summary>
        public string ResourceId { get; set; }

        #endregion Properties
    }
}