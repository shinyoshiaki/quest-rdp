/**
 * Copyright (c) 2017 The Campfire Union Inc - All Rights Reserved.
 *
 * Licensed under the MIT license. See LICENSE file in the project root for
 * full license information.
 *
 * Email:   info@campfireunion.com
 * Website: https://www.campfireunion.com
 */

using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using System;

namespace VRKeys
{

    /// <summary>
    /// Example use of VRKeys keyboard.
    /// </summary>
    public class DemoScene : MonoBehaviour
    {
        public Keyboard keyboard;

        public GameObject connect;
        public GameObject player;

        private void OnEnable()
        {
            keyboard.Enable();
            keyboard.SetPlaceholderMessage("Please enter your email address");

            keyboard.OnSubmit.AddListener(HandleSubmit);
            keyboard.OnCancel.AddListener(HandleCancel);
        }

        private void OnDisable()
        {

            keyboard.OnSubmit.RemoveListener(HandleSubmit);
            keyboard.OnCancel.RemoveListener(HandleCancel);

            keyboard.Disable();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (keyboard.disabled)
                {
                    keyboard.Enable();
                }
                else
                {
                    keyboard.Disable();
                }
            }

            if (keyboard.disabled)
            {
                return;
            }

            if (Input.GetKeyDown(KeyCode.Q))
            {
                keyboard.SetLayout(KeyboardLayout.Qwerty);
            }
            else if (Input.GetKeyDown(KeyCode.F))
            {
                keyboard.SetLayout(KeyboardLayout.French);
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                keyboard.SetLayout(KeyboardLayout.Dvorak);
            }
        }

        public void HandleSubmit(string text)
        {
            Debug.Log("input " + text);
            var offset = new Vector3(0, 1, 3);
            var position = player.transform.position +
               player.transform.up * offset.y +
               player.transform.right * offset.x +
               player.transform.forward * offset.z;
            var obj = Instantiate(connect, position, player.transform.rotation);

            var peer = obj.GetComponentInChildren<Connect>();
            peer.StartConnect(text);
            keyboard.DisableInput();
            StartCoroutine(SubmitEmail(text));
        }

        public void HandleCancel()
        {
            Debug.Log("Cancelled keyboard input!");
        }

        private IEnumerator SubmitEmail(string email)
        {
            keyboard.ShowInfoMessage("Sending lots of spam, please wait... ;)");
            yield return new WaitForSeconds(1f);
            keyboard.ShowSuccessMessage("Lots of spam sent to " + email);
            yield return new WaitForSeconds(1f);
            keyboard.SetText("");
            keyboard.EnableInput();
        }

    }
}