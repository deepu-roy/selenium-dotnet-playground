FROM public.ecr.aws/bitnami/node:16 AS dev

# https://docs.cypress.io/guides/continuous-integration/introduction#Dependencies
RUN apt-get update && \
    export DEBIAN_FRONTEND=noninteractive && \
    apt-get -y install --no-install-recommends \
        libgtk2.0-0 \
        libgtk-3-0 \
        libgbm-dev \
        libnotify-dev \
        libgconf-2-4 \
        libnss3 \
        libxss1 \
        libasound2 \
        libxtst6 xauth xvfb
        
RUN wget https://packages.microsoft.com/repos/edge/pool/main/m/microsoft-edge-stable/microsoft-edge-stable_110.0.1587.41-1_amd64.deb?brand=M102
RUN sudo apt install ./microsoft-edge-stable_110.0.1587.41-1_amd64.deb

RUN wget https://dl.google.com/linux/direct/google-chrome-stable_current_amd64.deb
RUN sudo dpkg -i google-chrome-stable_current_amd64.deb

RUN sudo rm google-chrome-stable_current_amd64.deb
RUN sudo rm microsoft-edge-stable_110.0.1587.41-1_amd64.deb