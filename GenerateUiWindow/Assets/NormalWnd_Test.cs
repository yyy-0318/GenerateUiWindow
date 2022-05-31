using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

    [Window("NormalWnd_Test", (int)EWindowType.Normal)]
    public class NormalWnd_Test: WindowBase 
    {  

	Text m_Name_Text;
	Image m_Bg_Image;
	GameObject m_Confirm_Btn;
	GameObject m_Root_Go;
	Slider m_Hp_Slider;
	InputField m_Test_InputField;
	ScrollRect m_Test_ScrollRect;


        protected override void AfterInit()
        {
            base.AfterInit();
		m_Name_Text = FindByPath<Text>("Name_Text");
		m_Bg_Image = FindByPath<Image>("Bg_Image");
		m_Confirm_Btn = FindByPath("Confirm_Btn");
		m_Root_Go = FindByPath<GameObject>("Root_Go");
		m_Hp_Slider = FindByPath<Slider>("Hp_Slider");
		m_Test_InputField = FindByPath<InputField>("Test_InputField");
		m_Test_ScrollRect = FindByPath<ScrollRect>("Test_ScrollRect");


        }
        public void RegisterButtonEvent()
        {
		RegisterEventClick(m_Confirm_Btn, OnConfirmClick);

        }
        protected override void AfterShow()
        {
            base.AfterShow();
        }
        protected override void BeforeClose()
        {
            base.BeforeClose();
        }
		public void OnConfirmClick<PointerEventData>(GameObject go, PointerEventData eventData){}

    }