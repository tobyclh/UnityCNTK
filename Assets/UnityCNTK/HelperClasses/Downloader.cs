using UnityEngine;
using CNTK;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Net;
using System.IO;
namespace UnityCNTK
{
    public static class Downloader
    {
        public enum pretrainedModel
        {
            Alex, AlexNetBS, VGG16, VGG19, ResNet18, ResNet34, ResNet50, ResNet101, ResNet152, GoogLeNet
        }


        // A collection of pretrained model hosted by Microsoft
        // download speed has been suboptimal at the time of testing
        public static readonly Dictionary<pretrainedModel, string> modelHost = new Dictionary<pretrainedModel, string>()
        {
            {pretrainedModel.Alex,  "https://www.cntk.ai/Models/AlexNet/AlexNet.model"},
            {pretrainedModel.AlexNetBS,  "https://www.cntk.ai/Models/AlexNet/AlexNet.model"},
            {pretrainedModel.VGG16, "https://www.cntk.ai/Models/Caffe_Converted/VGG16_ImageNet_Caffe.model"},
            {pretrainedModel.VGG19, "https://www.cntk.ai/Models/Caffe_Converted/VGG19_ImageNet_Caffe.model"},
            {pretrainedModel.AlexNetBS,  "https://www.cntk.ai/Models/AlexNet/AlexNetBS.model"},
            {pretrainedModel.GoogLeNet,  "https://www.cntk.ai/Models/CNTK_Pretrained/InceptionV3_ImageNet_CNTK.model"},
            {pretrainedModel.ResNet18,  "https://www.cntk.ai/Models/ResNet/ResNet_18.model"},
            {pretrainedModel.ResNet34,  "https://www.cntk.ai/Models/CNTK_Pretrained/ResNet34_ImageNet_CNTK.model"},
            {pretrainedModel.ResNet50,  "https://www.cntk.ai/Models/CNTK_Pretrained/ResNet50_ImageNet_CNTK.model"},
            {pretrainedModel.ResNet101,  "https://www.cntk.ai/Models/Caffe_Converted/ResNet101_ImageNet_Caffe.model"},
            {pretrainedModel.ResNet152,  "https://www.cntk.ai/Models/Caffe_Converted/ResNet152_ImageNet_Caffe.model"}
        };


        public static void DownloadPretrainedModel(pretrainedModel model, string relativePath)
        {
            string modelPath = System.IO.Path.Combine(Environment.CurrentDirectory, relativePath);
            if (!File.Exists(modelPath))
            {
                using (var client = new WebClient())
                {
                    var modelURL = modelHost[model];
                    Debug.Log("Downloading model from " + modelURL);
                    client.DownloadFile(modelURL, modelPath);
                    Debug.Log("Downloaded model at " + relativePath);
                }
            }
            else
            {
                Debug.Log("Model foudn at " + relativePath);
            }
        }


        public static void DownloadDataset()
        {

        }
    }
}
