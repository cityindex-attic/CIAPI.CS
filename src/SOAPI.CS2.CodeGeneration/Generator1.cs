// <copyright file="Generator1.cs" company="Microsoft">
//  Copyright © Microsoft. All Rights Reserved.
// </copyright>

namespace SOAPI.CS2.CodeGeneration
{
    using System;
    using T4Toolbox;

    public partial class Generator1 : Generator
    {
        protected override void RunCore()
        {

        }

        protected override void Validate()
        {
            this.Warning("Generator properties have not been validated");
        }
    }
}
