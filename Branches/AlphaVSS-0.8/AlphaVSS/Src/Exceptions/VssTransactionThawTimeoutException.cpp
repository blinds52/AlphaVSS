/* Copyright (c) 2008 Peter Palotas
 *  
 *  Permission is hereby granted, free of charge, to any person obtaining a copy
 *  of this software and associated documentation files (the "Software"), to deal
 *  in the Software without restriction, including without limitation the rights
 *  to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 *  copies of the Software, and to permit persons to whom the Software is
 *  furnished to do so, subject to the following conditions:
 *  
 *  The above copyright notice and this permission notice shall be included in
 *  all copies or substantial portions of the Software.
 *  
 *  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 *  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 *  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 *  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 *  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 *  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 *  THE SOFTWARE.
 */
#include "StdAfx.h"

// If not on vista or later, this error code is not defined, we define it to a bogus value.
#if ALPHAVSS_TARGET < ALPHAVSS_TARGET_WIN2008
#define VSS_E_TRANSACTION_THAW_TIMEOUT E_UNEXPECTED
#endif

namespace Alphaleonis { namespace Win32 { namespace Vss
{		
	VssTransactionThawTimeoutException::VssTransactionThawTimeoutException()
		: VssException(VSS_E_TRANSACTION_THAW_TIMEOUT, L"The system was unable to thaw the Distributed Transaction Coordinator (DTC) or the Kernel Transaction Manager (KTM).")
	{
	}

	VssTransactionThawTimeoutException::VssTransactionThawTimeoutException(String ^ message)
		: VssException(VSS_E_TRANSACTION_THAW_TIMEOUT, message)
	{
	}

	VssTransactionThawTimeoutException::VssTransactionThawTimeoutException(String ^ message, Exception ^ innerException)
		: VssException(message, innerException)
	{
	}

	VssTransactionThawTimeoutException::VssTransactionThawTimeoutException(System::Runtime::Serialization::SerializationInfo ^ info, System::Runtime::Serialization::StreamingContext context)
		: VssException(info, context)
	{
	}
}
} }

#if ALPHAVSS_TARGET < ALPHAVSS_TARGET_WIN2008
#undef VSS_E_REBOOT_REQUIRED 
#endif