// @ts-check
// `@type` JSDoc annotations allow editor autocompletion and type checking
// (when paired with `@ts-check`).
// There are various equivalent ways to declare your Docusaurus config.
// See: https://docusaurus.io/docs/api/docusaurus-config

import {themes as prismThemes} from 'prism-react-renderer';

/** @type {import('@docusaurus/types').Config} */
const config = {
  title: 'Easy Food',
  tagline: '',
  favicon: 'img/favicon.ico',

  // Set the production url of your site here
  url: 'https://5soat-acme.github.io',
  // Set the /<baseUrl>/ pathname under which your site is served
  // For GitHub pages deployment, it is often '/<projectName>/'
  baseUrl: '/easy-food/',

  // GitHub pages deployment config.
  // If you aren't using GitHub pages, you don't need these.
  organizationName: '5soat-acme', // Usually your GitHub org/user name.
  projectName: 'Easy Food', // Usually your repo name.
  deploymentBranch: 'gh-pages',
  trailingSlash: false,

  onBrokenLinks: 'throw',
  onBrokenMarkdownLinks: 'warn',

  // Even if you don't use internationalization, you can use this field to set
  // useful metadata like html lang. For example, if your site is Chinese, you
  // may want to replace "en" with "zh-Hans".
  i18n: {
    defaultLocale: 'pt-br',
    locales: ['pt-br']
  },

  presets: [
    [
      'classic',
      /** @type {import('@docusaurus/preset-classic').Options} */
      ({
        docs: {
          sidebarPath: './sidebars.js',
          // Please change this to your repo.
          // Remove this to remove the "edit this page" links.
          editUrl:
            'https://github.com/5soat-acme/easy-food/',
        },
        blog: {
          showReadingTime: true,
          // Please change this to your repo.
          // Remove this to remove the "edit this page" links.
          editUrl:
            'https://github.com/5soat-acme/easy-food/',
        },
        theme: {
          customCss: './src/css/custom.css',
        },
      }),
    ],
  ],

  themeConfig:
    /** @type {import('@docusaurus/preset-classic').ThemeConfig} */
    ({
      // Replace with your project's social card
      // image: 'img/docusaurus-social-card.jpg',
      navbar: {
        title: 'Easy Food',
        logo: {
          alt: 'Easy Food',
          src: 'img/logo.svg',
        },
        items: [
          {
            type: 'docSidebar',
            sidebarId: 'tutorialSidebar',
            position: 'left',
            label: 'Wiki',
          },
          {
            href: 'https://github.com/5soat-acme/easy-food',
            label: 'GitHub',
            position: 'right',
          },
        ],
      },
      footer: {
        style: 'dark',
        links: [
          {
            title: 'Docs',
            items: [
              {
                label: 'Mapa de Subdomínios',
                to: '/docs/modelo-estrategico/mapa-subdominios',
              },
              {
                label: 'Linguiagem Ubíqua',
                to: '/docs/modelo-estrategico/linguagem-ubiqua',
              },
              {
                label: 'Domain Storytteling',
                to: '/docs/modelo-estrategico/domain-storytteling',
              },
              {
                label: 'Event Storming',
                href: '/docs/modelo-estrategico/event-storming',
              },
              {
                label: 'Mapa de Contextos',
                to: '/docs/modelo-estrategico/mapa-contextos',
              },
            ],
          },
          {
            title: 'Autores',
            items: [
              {
                label: 'Lucas Fernandes',
                href: 'https://github.com/Lukiita',
              },
              {
                label: 'Luis Guilherme',
                href: 'https://github.com/Lguilhermeg',
              },
              {
                label: 'Pedro Barão',
                href: 'https://github.com/pedrobarao',
              },
              {
                label: 'Rafael Casadei',
                href: 'https://github.com/RafaCasadei',
              },
              {
                label: 'Rodrigo Lima',
                href: 'https://github.com/Rodrigo1895',
              },
            ],
          },
          {
            title: 'Mais',
            items: [
              {
                label: 'GitHub',
                href: 'https://github.com/5soat-acme/easy-food',
              },
            ],
          },
        ],
        copyright: `Copyright © ${new Date().getFullYear()} Easy Food, Inc. ACME.`,
      },
      prism: {
        theme: prismThemes.github,
        darkTheme: prismThemes.dracula,
      },
    }),
};

export default config;
